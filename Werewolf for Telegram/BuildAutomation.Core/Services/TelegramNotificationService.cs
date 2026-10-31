using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Requests;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace BuildAutomation.Core.Services;

public interface ITelegramNotificationService
{
    Task SendAsync(string message, ParseMode? parseMode = null, InlineKeyboardMarkup? replyMarkup = null,
        bool disableWebPagePreview = false, CancellationToken cancellationToken = default);
}

public sealed class TelegramNotificationService : ITelegramNotificationService
{
    private readonly TelegramBotClient _bot;
    private readonly long _groupId;

    public TelegramNotificationService(IConfiguration configuration)
    {
        var token = configuration["BuildAutomation:TelegramApiToken"];
        if (string.IsNullOrWhiteSpace(token))
        {
            throw new InvalidOperationException("BuildAutomation:TelegramApiToken is not configured.");
        }

        if (!long.TryParse(configuration["BuildAutomation:TelegramGroupId"], out _groupId))
        {
            throw new InvalidOperationException("BuildAutomation:TelegramGroupId is not configured with a valid chat ID.");
        }

        _bot = new TelegramBotClient(token);
    }

    public Task SendAsync(string message, ParseMode? parseMode = null, InlineKeyboardMarkup? replyMarkup = null,
        bool disableWebPagePreview = false, CancellationToken cancellationToken = default)
    {
        var request = new SendMessageRequest
        {
            ChatId = _groupId,
            Text = message,
            LinkPreviewOptions = new LinkPreviewOptions { IsDisabled = disableWebPagePreview },
            ReplyMarkup = replyMarkup
        };

        if (parseMode.HasValue)
        {
            request.ParseMode = parseMode.Value;
        }

        return _bot.SendRequest(request, cancellationToken);
    }
}
