using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Requests;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Werewolf_Node.Helpers;

internal static class TelegramBotClientExtensions
{
    public static Task<User> GetMeAsync(this TelegramBotClient bot, CancellationToken cancellationToken = default) =>
        bot.SendRequest(new GetMeRequest(), cancellationToken);

    public static Task<ChatMember> GetChatMemberAsync(this TelegramBotClient bot, ChatId chatId, long userId,
        CancellationToken cancellationToken = default) =>
        bot.SendRequest(new GetChatMemberRequest { ChatId = chatId, UserId = userId }, cancellationToken);

    public static Task<bool> BanChatMemberAsync(this TelegramBotClient bot, ChatId chatId, long userId,
        CancellationToken cancellationToken = default) =>
        bot.SendRequest(new BanChatMemberRequest { ChatId = chatId, UserId = userId }, cancellationToken);

    public static Task<bool> UnbanChatMemberAsync(this TelegramBotClient bot, ChatId chatId, long userId,
        CancellationToken cancellationToken = default) =>
        bot.SendRequest(new UnbanChatMemberRequest { ChatId = chatId, UserId = userId }, cancellationToken);

    public static Task<int> GetChatMemberCountAsync(this TelegramBotClient bot, ChatId chatId,
        CancellationToken cancellationToken = default) =>
        bot.SendRequest(new GetChatMemberCountRequest { ChatId = chatId }, cancellationToken);

    public static Task<UserProfilePhotos> GetUserProfilePhotosAsync(this TelegramBotClient bot, long userId,
        int? offset = null, int? limit = null, CancellationToken cancellationToken = default) =>
        bot.SendRequest(new GetUserProfilePhotosRequest
        {
            UserId = userId,
            Offset = offset,
            Limit = limit
        }, cancellationToken);

    public static Task<TGFile> GetFileAsync(this TelegramBotClient bot, string fileId,
        CancellationToken cancellationToken = default) =>
        bot.SendRequest(new GetFileRequest { FileId = fileId }, cancellationToken);

    public static Task<Message> SendTextMessageAsync(this TelegramBotClient bot, ChatId chatId, string text,
        ReplyMarkup? replyMarkup = null, ParseMode? parseMode = null,
        bool disableWebPagePreview = false, bool disableNotification = false,
        int? replyToMessageId = null, CancellationToken cancellationToken = default)
    {
        var request = new SendMessageRequest
        {
            ChatId = chatId,
            Text = text,
            ReplyMarkup = replyMarkup,
            LinkPreviewOptions = new LinkPreviewOptions { IsDisabled = disableWebPagePreview },
            DisableNotification = disableNotification,
            ReplyParameters = replyToMessageId.HasValue
                ? new ReplyParameters { MessageId = replyToMessageId.Value }
                : null
        };
        if (parseMode.HasValue) request.ParseMode = parseMode.Value;
        return bot.SendRequest(request, cancellationToken);
    }

    public static Task<Message> EditMessageTextAsync(this TelegramBotClient bot, ChatId chatId, int messageId,
        string text, ParseMode? parseMode = null, bool disableWebPagePreview = false,
        InlineKeyboardMarkup? replyMarkup = null, CancellationToken cancellationToken = default)
    {
        var request = new EditMessageTextRequest
        {
            ChatId = chatId,
            MessageId = messageId,
            Text = text,
            LinkPreviewOptions = new LinkPreviewOptions { IsDisabled = disableWebPagePreview },
            ReplyMarkup = replyMarkup
        };
        if (parseMode.HasValue) request.ParseMode = parseMode.Value;
        return bot.SendRequest(request, cancellationToken);
    }

    public static Task<Message> EditMessageReplyMarkupAsync(this TelegramBotClient bot, ChatId chatId, int messageId,
        InlineKeyboardMarkup? replyMarkup = null, CancellationToken cancellationToken = default) =>
        bot.SendRequest(new EditMessageReplyMarkupRequest
        {
            ChatId = chatId,
            MessageId = messageId,
            ReplyMarkup = replyMarkup
        }, cancellationToken);

    public static Task<bool> DeleteMessageAsync(this TelegramBotClient bot, ChatId chatId, int messageId,
        CancellationToken cancellationToken = default) =>
        bot.SendRequest(new DeleteMessageRequest { ChatId = chatId, MessageId = messageId }, cancellationToken);

    public static Task<bool> AnswerCallbackQueryAsync(this TelegramBotClient bot, string callbackQueryId,
        string? text = null, bool showAlert = false, CancellationToken cancellationToken = default) =>
        bot.SendRequest(new AnswerCallbackQueryRequest
        {
            CallbackQueryId = callbackQueryId,
            Text = text,
            ShowAlert = showAlert
        }, cancellationToken);
}
