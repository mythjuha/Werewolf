using System.Text.Json;
using Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;
using Telegram.Bot;
using DonationSite.Core.Models;

namespace DonationSite.Core.Controllers.Webhook;

[ApiController]
[Route("webhook/[controller]")]
public sealed class StripeController : ControllerBase
{
    private readonly WWCoreContext _db;
    private readonly IConfiguration _configuration;
    private readonly TelegramBotClient _bot;
    private readonly ILogger<StripeController> _logger;

    public StripeController(WWCoreContext db, IConfiguration configuration, ILogger<StripeController> logger)
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
        var payload = await reader.ReadToEndAsync(cancellationToken);
        var signature = Request.Headers["Stripe-Signature"].FirstOrDefault();
        var secret = RequiredSetting(_configuration, "StripeSecret");

        try
        {
            var stripeEvent = EventUtility.ConstructEvent(payload, signature, secret);
            if (stripeEvent.Type != "invoice.payment_succeeded" || stripeEvent.Data.Object is not Invoice invoice)
            {
                return BadRequest("Unexpected Event Type");
            }

            var separator = invoice.Number?.IndexOf('-') ?? -1;
            if (separator <= 0 || !long.TryParse(invoice.Number![..separator], out var userId))
            {
                return BadRequest("Invalid User ID");
            }

            var amount = Convert.ToInt32(invoice.Total / 100);
            var player = await _db.Players.FirstOrDefaultAsync(x => x.TelegramId == userId, cancellationToken);
            if (player is null)
            {
                return BadRequest("Invalid User ID");
            }

            var oldLevel = player.DonationLevel ?? 0;
            player.DonationLevel = oldLevel + amount;
            var level = player.DonationLevel ?? 0;
            var badge = GetBadge(player.Founder, level);
            await _bot.SendMessage(userId, $"Successfully received ${amount} from you! YAY!\nTotal Donated: ${level}\nCurrent Badge (ingame): {badge}", cancellationToken: cancellationToken);

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
        catch (StripeException exception)
        {
            _logger.LogWarning(exception, "Invalid Stripe webhook payload");
            return BadRequest(exception.Message);
        }
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
