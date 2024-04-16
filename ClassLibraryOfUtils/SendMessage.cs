using Telegram.Bot;
using Telegram.Bot.Types;

namespace ClassLibraryOfUtils;
/// <summary>
/// A class for sending text messages or stickers to the chat room.
/// </summary>
public class SendMessage
{
    private readonly ITelegramBotClient _botClient;
    private readonly CancellationToken _cancellationToken;
    public SendMessage(ITelegramBotClient botClient, CancellationToken cancellationToken)
    {
        _botClient = botClient;
        _cancellationToken = cancellationToken;
    }

    public SendMessage(){}

    /// <summary>
    /// The method for sending ordinary text messages to the chat.
    /// </summary>
    async public Task SendText(string text, long chatId)
    {
        await _botClient.SendTextMessageAsync(
            chatId: chatId,
            text: text,
            cancellationToken: _cancellationToken);
    }

    /// <summary>
    /// The method for sending text messages with reply to the chat.
    /// </summary>
    async public Task SendReplyMessage(string text, long chatId, int messageId)
    {
        await _botClient.SendTextMessageAsync(
            chatId: chatId,
            text: text,
            replyToMessageId: messageId,
            cancellationToken: _cancellationToken);
    }

    /// <summary>
    /// The method for sending sticker to the chat.
    /// </summary>
    async public Task SendSticker(long chatId)
    {
        await _botClient.SendStickerAsync(
            chatId: chatId,
            sticker: InputFile.FromFileId("CAACAgIAAxkBAAEENq1l_JacBJ5GYeJm30MSUZ5KWFKw0AACxC0AAmzZwUjIy8WIHOUdpjQE"),
            cancellationToken: _cancellationToken);
    }
}

