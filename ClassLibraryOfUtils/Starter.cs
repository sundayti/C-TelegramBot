using Telegram.Bot;
using Telegram.Bot.Polling;

namespace ClassLibraryOfUtils;

/// <summary>
/// The class to start the bot.
/// </summary>
public static class Starter
{
    /// <summary>
    /// The method to start the bot.
    /// </summary>
    async public static Task Satrt(ITelegramBotClient botClient, Handlers handlers,
        CancellationTokenSource cts, ReceiverOptions receiverOptions)
    {
        botClient.StartReceiving(
            updateHandler: handlers.HandleUpdateAsync,
            pollingErrorHandler: PollingErrors.HandlePollingErrorAsync,
            receiverOptions: receiverOptions,
            cancellationToken: cts.Token
        );

        // So that the bot keeps trying to connect to API.
        while (true)
        {
            // To prevent the program from throwing out RequestException we using try.
            try
            {
                var me = await botClient.GetMeAsync(cancellationToken: cts.Token);
                Console.WriteLine($"Start listening for @{me.Username}");
                break;
            }
            catch
            {
                // ignored
            }
        }
    }
}