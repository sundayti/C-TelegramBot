using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using ClassLibraryOfUtils;

namespace TgBot
{
    internal static class Program
    {
        static async Task Main()
        {
            Logger.CreateLogger();
            // TODO Вставьте свой api
            var botClient = new TelegramBotClient("");
            using CancellationTokenSource cts = new();

            Dictionary<long, UserData> users = new Dictionary<long, UserData>();
            Handlers handlers = new Handlers(botClient, users, cts.Token);

            // StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
            ReceiverOptions receiverOptions = new()
            {
                AllowedUpdates = Array.Empty<UpdateType>()
            };

            // Start the bot.
            await Starter.Satrt(botClient, handlers, cts, receiverOptions);

            Console.ReadLine();

            // Send cancellation request to stop bot
            cts.Cancel();
        }
    }
}