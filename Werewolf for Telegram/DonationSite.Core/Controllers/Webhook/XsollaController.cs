using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Database;
using DonationSite.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Exceptions;

namespace DonationSite.Core.Controllers.Webhook;

[ApiController]
[Route("webhook/[controller]")]
public sealed class XsollaController : ControllerBase
{
    private readonly WWCoreContext _db;
    private readonly IConfiguration _configuration;
    private readonly TelegramBotClient _bot;
    private readonly ILogger<XsollaController> _logger;

    public XsollaController(WWCoreContext db, IConfiguration configuration, ILogger<XsollaController> logger)
    {
        _db = db;
        _configuration = configuration;
        _bot = new TelegramBotClient(RequiredSetting(configuration, "TelegramAPIToken"));
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> Post(CancellationToken cancellationToken)
    {
        using var reader = new StreamReader(Request.Body);
        var body = await reader.ReadToEndAsync(cancellationToken);
        var authorization = Request.Headers.Authorization.FirstOrDefault();
        var signature = authorization?.StartsWith("Signature ", StringComparison.OrdinalIgnoreCase) == true
            ? authorization[10..]
            : authorization;
        var expected = ComputeSignature(body + RequiredSetting(_configuration, "XsollaProjectSecretKey"));

        if (!CryptographicOperations.FixedTimeEquals(
                Encoding.UTF8.GetBytes(expected), Encoding.UTF8.GetBytes(signature ?? string.Empty)))
        {
            return Error("INVALID_SIGNATURE");
        }

        try
        {
            var payload = JsonSerializer.Deserialize<XsollaEvent>(body);
            switch (payload?.NotificationType)
            {
                case "user_validation":
                    return await ValidateUserAsync(payload.User?.Id, cancellationToken);
                case "payment":
                    return await ProcessPaymentAsync(payload, cancellationToken);
                case "refund":
                    return await ProcessRefundAsync(payload, cancellationToken);
                default:
                    return Ok();
            }
        }
        catch (ApiRequestException exception)
        {
            _logger.LogError(exception, "Telegram rejected an Xsolla webhook notification");
            return StatusCode(exception.ErrorCode, exception.Message);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error processing Xsolla webhook");
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    private async Task<IActionResult> ValidateUserAsync(string? value, CancellationToken cancellationToken)
    {
        if (!long.TryParse(value, out var userId) || userId == 0 ||
            !await _db.Players.AnyAsync(x => x.TelegramId == userId, cancellationToken))
        {
            return Error("INVALID_USER");
        }

        return Ok();
    }

    private async Task<IActionResult> ProcessPaymentAsync(XsollaEvent payload, CancellationToken cancellationToken)
    {
        if (!long.TryParse(payload.User?.Id, out var userId) || userId == 0)
        {
            return Error("INVALID_USER");
        }

        var player = await _db.Players.FirstOrDefaultAsync(x => x.TelegramId == userId, cancellationToken);
        if (player is null)
        {
            return Error("INVALID_USER");
        }

        var amount = (int)(payload.Purchase?.VirtualCurrency?.Amount ?? 0);
        var oldLevel = player.DonationLevel ?? 0;
        player.DonationLevel = oldLevel + amount;
        var level = player.DonationLevel ?? 0;
        await _bot.SendMessage(userId, $"Successfully received ${amount} from you! YAY!\nTotal Donated: ${level}\nCurrent Badge (ingame): {GetBadge(player.Founder, level)}", cancellationToken: cancellationToken);

        if (level > 10 && oldLevel < 10)
        {
            player.GifPurchased = true;
            var data = DeserializeGifData(player.CustomGifSet);
            if (!data.HasPurchased)
            {
                await _bot.SendMessage(userId, "Congratulations! You have unlocked Custom Gif Packs :)\nUse /customgif to build your pack, /submitgif to submit for approval", cancellationToken: cancellationToken);
            }
            data.HasPurchased = true;
            player.CustomGifSet = JsonSerializer.Serialize(data);
        }

        await _db.SaveChangesAsync(cancellationToken);
        return Ok();
    }

    private async Task<IActionResult> ProcessRefundAsync(XsollaEvent payload, CancellationToken cancellationToken)
    {
        if (!long.TryParse(payload.User?.Id, out var userId) || userId == 0)
        {
            return Error("INVALID_USER");
        }

        await _bot.SendMessage(userId, $"Your donation did not pass through Xsolla because of: {payload.RefundDetails?.Reason}. If you have any questions please go to @werewolfsupport.", cancellationToken: cancellationToken);
        return Ok();
    }

    private ObjectResult Error(string reason) => StatusCode(StatusCodes.Status403Forbidden, new { error = new { code = reason } });

    private static string ComputeSignature(string input)
    {
        var hash = SHA1.HashData(Encoding.UTF8.GetBytes(input));
        return Convert.ToHexString(hash).ToLowerInvariant();
    }

    private static CustomGifData DeserializeGifData(string? json) =>
        string.IsNullOrWhiteSpace(json)
            ? new CustomGifData()
            : JsonSerializer.Deserialize<CustomGifData>(json) ?? new CustomGifData();

    private static string GetBadge(bool? founder, int level)
    {
        var badge = level >= 100 ? " 🥇" : level >= 50 ? " 🥈" : level >= 10 ? " 🥉" : string.Empty;
        return founder == true ? badge + "💎" : badge;
    }

    private static string RequiredSetting(IConfiguration configuration, string key) =>
        configuration[key] ?? throw new InvalidOperationException($"Missing required configuration: {key}");
}
