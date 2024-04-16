using Telegram.Bot;
using Telegram.Bot.Types;

namespace ClassLibraryOfUtils;
/// <summary>
/// The clss for send the file to user.
/// </summary>
public class Writer
{
    private readonly ITelegramBotClient _botClient;
    private readonly CancellationToken _cancellationToken;
    
    public Writer(ITelegramBotClient botClient, CancellationToken cancellationToken)
    {
        _cancellationToken = cancellationToken;
        _botClient = botClient;
    }
    public Writer(){}
    /// <summary>
    /// The method for send the file to user.
    /// </summary>
    public async Task Write(string extension, List<WiFiPark> wiFiParks, long chatId)
    {
        if (extension == "JSON")
        {
            JsonProcesses jp = new JsonProcesses();
            await using Stream stream = await jp.Write(wiFiParks);
            await _botClient.SendDocumentAsync(
                chatId: chatId,
                document: InputFile.FromStream(
                    stream: stream, 
                    fileName: "wifi-parks.json"),
                caption: "Your json file",
                cancellationToken: _cancellationToken);
        }
        else
        {
            CsvProcesses csv = new CsvProcesses();
            await using Stream stream = await csv.Write(wiFiParks);
            await _botClient.SendDocumentAsync(
                chatId: chatId,
                document: InputFile.FromStream(
                    stream: stream,
                    fileName: "wifi-parks.csv"),
                caption: "Your csv file",
                cancellationToken: _cancellationToken);
        }
        
        
    }
}