using Telegram.Bot;
using Telegram.Bot.Types;
namespace ClassLibraryOfUtils;

/// <summary>
/// Class for reading data from a sended file.
/// </summary>
public class Reader
{
    private readonly ITelegramBotClient _botClient;
    private readonly CancellationToken _cancellationToken;
    public Reader(ITelegramBotClient botClient, CancellationToken cancellationToken)
    {
        _cancellationToken = cancellationToken;
        _botClient = botClient;
    }
    public Reader(){}
    /// <summary>
    /// The method reads data from the file using the required class.
    /// </summary>
    public async Task<List<WiFiPark>?> Read(MemoryStream stream, Document document) 
    {
        
        await _botClient.GetInfoAndDownloadFileAsync(
            fileId: document.FileId,
            destination: stream,
            cancellationToken: _cancellationToken);
        
        if (Path.GetExtension(document.FileName) == ".json")
        {
            JsonProcesses jp = new JsonProcesses();
            return jp.Read(stream);
        }

        if (Path.GetExtension(document.FileName) == ".csv")
        {
            CsvProcesses cp = new CsvProcesses();
            return cp.Read(stream);
        }

        return null;

    }
    
    
}