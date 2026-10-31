using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Telegram.Bot;
using Telegram.Bot.Requests;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.Payments;
using Telegram.Bot.Types.ReplyMarkups;

namespace Werewolf_Control.Helpers;

internal static class TelegramBotClientExtensions
{
    public static Task<Message> SendTextMessageAsync(this TelegramBotClient bot, ChatId chatId, string text,
        ReplyMarkup? replyMarkup = null, ParseMode? parseMode = null, bool disableWebPagePreview = false,
        bool disableNotification = false, int? replyToMessageId = null, int? messageThreadId = null,
        CancellationToken cancellationToken = default)
    {
        var request = new SendMessageRequest
        {
            ChatId = chatId, Text = text, ReplyMarkup = replyMarkup,
            LinkPreviewOptions = new LinkPreviewOptions { IsDisabled = disableWebPagePreview },
            DisableNotification = disableNotification,
            MessageThreadId = messageThreadId,
            ReplyParameters = replyToMessageId.HasValue ? new ReplyParameters { MessageId = replyToMessageId.Value } : null
        };
        if (parseMode.HasValue) request.ParseMode = parseMode.Value;
        return bot.SendRequest(request, cancellationToken);
    }

    public static Task<User> GetMeAsync(this TelegramBotClient bot, CancellationToken cancellationToken = default) =>
        bot.SendRequest(new GetMeRequest(), cancellationToken);

    public static Task<bool> AnswerCallbackQueryAsync(this TelegramBotClient bot, string callbackQueryId,
        string? text = null, bool showAlert = false, CancellationToken cancellationToken = default) =>
        bot.SendRequest(new AnswerCallbackQueryRequest { CallbackQueryId = callbackQueryId, Text = text, ShowAlert = showAlert }, cancellationToken);

    public static Task<Message> EditMessageReplyMarkupAsync(this TelegramBotClient bot, ChatId chatId, int messageId,
        InlineKeyboardMarkup? replyMarkup = null, CancellationToken cancellationToken = default) =>
        bot.SendRequest(new EditMessageReplyMarkupRequest { ChatId = chatId, MessageId = messageId, ReplyMarkup = replyMarkup }, cancellationToken);

    public static Task<bool> AnswerPreCheckoutQueryAsync(this TelegramBotClient bot, string preCheckoutQueryId,
        CancellationToken cancellationToken = default) =>
        bot.SendRequest(new AnswerPreCheckoutQueryRequest { PreCheckoutQueryId = preCheckoutQueryId, Ok = true }, cancellationToken);

    public static Task<Message> EditMessageTextAsync(this TelegramBotClient bot, ChatId chatId, int messageId, string text,
        ParseMode? parseMode = null, bool disableWebPagePreview = false, InlineKeyboardMarkup? replyMarkup = null,
        CancellationToken cancellationToken = default)
    {
        var request = new EditMessageTextRequest
        {
            ChatId = chatId, MessageId = messageId, Text = text, ReplyMarkup = replyMarkup,
            LinkPreviewOptions = new LinkPreviewOptions { IsDisabled = disableWebPagePreview }
        };
        if (parseMode.HasValue) request.ParseMode = parseMode.Value;
        return bot.SendRequest(request, cancellationToken);
    }

    public static Task<TGFile> GetFileAsync(this TelegramBotClient bot, string fileId, CancellationToken cancellationToken = default) =>
        bot.SendRequest(new GetFileRequest { FileId = fileId }, cancellationToken);

    public static Task<ChatMember> GetChatMemberAsync(this TelegramBotClient bot, ChatId chatId, long userId, CancellationToken cancellationToken = default) =>
        bot.SendRequest(new GetChatMemberRequest { ChatId = chatId, UserId = userId }, cancellationToken);

    public static Task<bool> BanChatMemberAsync(this TelegramBotClient bot, ChatId chatId, long userId, CancellationToken cancellationToken = default) =>
        bot.SendRequest(new BanChatMemberRequest { ChatId = chatId, UserId = userId }, cancellationToken);

    public static Task<bool> UnbanChatMemberAsync(this TelegramBotClient bot, ChatId chatId, long userId, CancellationToken cancellationToken = default) =>
        bot.SendRequest(new UnbanChatMemberRequest { ChatId = chatId, UserId = userId }, cancellationToken);

    public static Task<bool> LeaveChatAsync(this TelegramBotClient bot, ChatId chatId, CancellationToken cancellationToken = default) =>
        bot.SendRequest(new LeaveChatRequest { ChatId = chatId }, cancellationToken);

    public static Task<ChatMember[]> GetChatAdministratorsAsync(this TelegramBotClient bot, ChatId chatId, CancellationToken cancellationToken = default) =>
        bot.SendRequest(new GetChatAdministratorsRequest { ChatId = chatId }, cancellationToken);

    public static Task<Message> SendDocumentAsync(this TelegramBotClient bot, ChatId chatId, InputFile document,
        string? caption = null, int? messageThreadId = null, CancellationToken cancellationToken = default) =>
        bot.SendRequest(new SendDocumentRequest { ChatId = chatId, Document = document, Caption = caption, MessageThreadId = messageThreadId }, cancellationToken);

    public static Task<Message> SendPhotoAsync(this TelegramBotClient bot, ChatId chatId, InputFile photo,
        int? messageThreadId = null, CancellationToken cancellationToken = default) =>
        bot.SendRequest(new SendPhotoRequest { ChatId = chatId, Photo = photo, MessageThreadId = messageThreadId }, cancellationToken);

    public static Task DownloadFileAsync(this TelegramBotClient bot, string filePath, Stream destination,
        CancellationToken cancellationToken = default) => bot.DownloadFile(filePath, destination, cancellationToken);

    public static Task<bool> AnswerInlineQueryAsync(this TelegramBotClient bot, string inlineQueryId,
        IEnumerable<InlineQueryResult> results, int cacheTime = 300, bool isPersonal = false,
        CancellationToken cancellationToken = default) =>
        bot.SendRequest(new AnswerInlineQueryRequest { InlineQueryId = inlineQueryId, Results = results.ToArray(), CacheTime = cacheTime, IsPersonal = isPersonal }, cancellationToken);

    public static Task<Message> SendInvoiceAsync(this TelegramBotClient bot, ChatId chatId, string title, string description,
        string payload, string providerToken, string currency, IEnumerable<LabeledPrice> prices, string? startParameter = null,
        CancellationToken cancellationToken = default) =>
        bot.SendRequest(new SendInvoiceRequest
        {
            ChatId = chatId, Title = title, Description = description, Payload = payload,
            ProviderToken = providerToken, Currency = currency, Prices = prices.ToArray(), StartParameter = startParameter
        }, cancellationToken);

    public static Task<bool> DeleteMessageAsync(this TelegramBotClient bot, ChatId chatId, int messageId, CancellationToken cancellationToken = default) =>
        bot.SendRequest(new DeleteMessageRequest { ChatId = chatId, MessageId = messageId }, cancellationToken);
}
