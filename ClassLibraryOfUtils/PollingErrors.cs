using Serilog;
using Telegram.Bot.Exceptions;
using Telegram.Bot;

namespace ClassLibraryOfUtils;
public static class PollingErrors
{
    private static readonly DateTime LastRequestErrorDate = new(0, 0, 0);
    /// <summary>
    /// Method to start the polling errors.
    /// </summary>
    public static Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        // To prevent the log from displaying a hundred RequestExceptions per second,
        // we check when the last error occurred. 
        if (exception.GetType() == typeof(RequestException) &&
            DateTime.Now - LastRequestErrorDate < new TimeSpan(0, 0, 10))
        {
            return Task.CompletedTask;
        }
        Log.Error($"At {DateTime.Now} {exception.Message}");
        
        return Task.CompletedTask;
    }
}

