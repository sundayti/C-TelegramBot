using Serilog;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ClassLibraryOfUtils;
/// <summary>
/// Cass handling all updates in the bot.
/// </summary>
public class Handlers
{
    private readonly Reader _reader = null!;
    private readonly Writer _writer = null!;
    private readonly SendMessage _sender = null!;
    private readonly MenuSender _menuSender = null!;
    private readonly Dictionary<long, UserData> _users = null!;
    private readonly SampleProcesses _sampler = null!;
    public Handlers(){}
    public Handlers(ITelegramBotClient botClient, Dictionary<long, UserData> users, 
        CancellationToken cancellationToken)
    {
        _users = users;
        _reader = new Reader(botClient, cancellationToken);
        _writer = new Writer(botClient, cancellationToken);
        _sender = new SendMessage(botClient, cancellationToken);
        _menuSender = new MenuSender(botClient, cancellationToken);
        _sampler = new SampleProcesses(users, _menuSender);
    }

    /// <summary>
    /// The method recognizes the type of update and launches the necessary handler.
    /// </summary>
    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Message is not { } message) goto Callback;
        if (message.From is null) return;
        var chatId = message.Chat.Id;
        var messageId = message.MessageId;
        if (!_users.ContainsKey(chatId))
        {
            _users.Add(chatId, new UserData());
        }
        var currentUserData = _users[chatId];
        
        // Check if the received update is a text message.
        if (message.Text is {} text)
        {
            text = text.Trim().Replace('\r',' ').Replace('\n', ' ').Replace("  ", " ");
            Log.Information($"At {DateTime.Now} received message in chat {chatId}: {text}");
            text = text.ToLower();
            await TextHandler(chatId, messageId, currentUserData, text);
            return;
        }
        // Check if the received update is a document.
        if (message.Document is { } document)
        {
            await DocumentHandler(messageId, document, chatId);
            if (update.Message.Document is { })
                Log.Information($"At {DateTime.Now} received document in chat {chatId}: {document.FileName}");
            return;
        }

        Callback:
            await CallbackQueryHandler(update);
    }
    
    /// <summary>
    /// The method handing text messages.
    /// </summary>
    private async Task TextHandler(long chatId, int messageId, UserData currentUserData,string text)
    {
        if (text == "/start")
        {
            // Ask the user to send the file.
            await _sender.SendText("Send the data file for processing", chatId);
        }
        // If the user selected a sampling by one of the fields
        // in the previous action, then run this sample.
        else if (currentUserData.IsSampleByCoverageArea)
        {
            await _sampler.SampleByCoverageArea(chatId, text, _sender, messageId);
        }
        else if (currentUserData.IsSampleByAdmArea)
        {
            await _sampler.SampleByAdmArea(chatId, text);
        }
        else if (currentUserData.IsSampleByParkName)
        {
            await _sampler.SampleByParkName(chatId, text);
        }
    }

    /// <summary>
    /// Method for handling all callbacks updates.
    /// </summary>
    private async Task CallbackQueryHandler(Update update)
    {
        // Check if there is a callback in update.
        if (update.CallbackQuery is not { } callback) return;
        if (callback.Message is not { } callbackMessage) return;
        
        long chatId = callbackMessage.Chat.Id;
        int messageId = callbackMessage.MessageId;

        var currentWiFiParks = _users[chatId].WiFiParks;
        if (currentWiFiParks == null) { return; }
        Log.Information($"At {DateTime.Now} received callback in chat {chatId}: {callback.Data}");
        
        // Checking for different callbacks.
        await MenuCallbackHandler(chatId, messageId, callback.Data ?? "");
        await SampleCallbackHandler(chatId, messageId, callback.Data ?? "");
        await SortCallbackHandler(chatId, messageId, callback.Data ?? "");
        await OtherCallbackHandler(chatId, messageId, callback.Data ?? "");

        await CallbackSamplingHandler(callback.Data ?? "", _users[chatId], chatId, messageId);
    }

    /// <summary>
    /// The method to handle Callback from the main menu.
    /// </summary>
    async private Task MenuCallbackHandler(long chatId, int messageId, string callbackData)
    {
        switch (callbackData)
        {
            case "Sampling":
                await _menuSender.SendSampleMenu(chatId, messageId, _users[chatId]);
                break;
            case "Sorting":
                await _menuSender.SortingMenu(chatId, messageId, _users[chatId]);
                break;
            case "Upload a new file":
                await _sender.SendText("Please, send a new data file", chatId);
                break;
            case "Download the processed file":
                await _menuSender.AskTypeFile(chatId, messageId, _users[chatId]);
                break;
        }
    }
    
    /// <summary>
    /// The method to handle Callback from the SampleMenu.
    /// </summary>
    async private Task SampleCallbackHandler(long chatId, int messageId, string callbackData)
    {
        switch (callbackData)
        {
            case "CoverageArea":
                _users[chatId].IsSampleByCoverageArea = true;
                await _menuSender.SelectCoverageArea(chatId, messageId, _users[chatId]);
                break;
            case "ParkName":
                _users[chatId].IsSampleByParkName = true;
                await _menuSender.SelectParkName(chatId, messageId, _users[chatId]);
                break;
            case "AdmArea and CoverageArea":
                _users[chatId].IsSampleByAdmArea = true;
                await _menuSender.SelectAdmArea(chatId, messageId, _users[chatId]);
                break;
        }
    }
    /// <summary>
    /// The method to handle Callback from the  SortMenu.
    /// </summary>
    async private Task SortCallbackHandler(long chatId, int messageId, string callbackData)
    {
        switch (callbackData)
        {
            case "CoverageArea in ascending order":
                _users[chatId].WiFiParks = _users[chatId].WiFiParks!.OrderBy(x => x.CoverageArea).ToList();
                _users[chatId].Text += "\nSort by CoverageArea";
                await _menuSender.SendMainMenu(chatId, messageId, _users[chatId]);
                break;
            case "Name by alphabetical order":
                _users[chatId].WiFiParks = _users[chatId].WiFiParks!.OrderBy(x => x.Name).ToList();
                _users[chatId].Text += "\nSort by Name";
                await _menuSender.SendMainMenu(chatId, messageId, _users[chatId]);
                break;
        }
    }
    
    /// <summary>
    /// The method to handle Callback from the select format menu and
    /// handle the callback "Back".
    /// </summary>
    async private Task OtherCallbackHandler(long chatId, int messageId, string callbackData)
    {
        switch (callbackData)
        {
            case "CSV":
            case "JSON":
                await _writer.Write(callbackData, _users[chatId].WiFiParks ?? WiFiPark.Empty(), chatId);
                break;
            case "Back":
            case "Nothing": 
                await _menuSender.SendMainMenu(chatId, messageId, _users[chatId]);
                _users[chatId].ToDefault();
                break;
        }
    }
    
    /// <summary>
    /// Method to handle callback with selected field for sampling.
    /// </summary>
    async private Task CallbackSamplingHandler(string callbackData, UserData userData, long chatId, int messageId)
    {
        if (!int.TryParse(callbackData, out int index)) {return;}
        
        // If the user selected a sampling by one of the fields
        // in the previous action, then run this sample.
        if (userData.IsSampleByAdmArea && index < userData.AdmAreas.Count)
        {
            await _sampler.SampleByAdmArea(chatId, messageId, index);
        }
        else if (userData.IsSampleByCoverageArea && index < userData.Coverages.Count)
        {
            await _sampler.SampleByCoverageArea(chatId, messageId, index);
        }
        else if(userData.IsSampleByParkName && index < userData.ParkNames.Count)
        {
            await _sampler.SampleByParkName(chatId, messageId, index);
        }
    }

    /// <summary>
    /// Method for to handle a document.
    /// </summary>
    private async Task DocumentHandler(int messageId, Document document, long chatId)
    {
        // Convert data from file to list of WiFiPark.
        await using MemoryStream stream = new();
        _users[chatId].WiFiParks = _reader.Read(stream, document).Result;
        
        if (_users[chatId].WiFiParks == null)
        {
            // Notify the user if the document contains incorrect data.
            await _sender.SendReplyMessage("Incorrect file format", chatId, messageId);
            await _sender.SendSticker(chatId);
        }
        else
        {
            _users[chatId].Text = "";
            await _menuSender.SendMainMenu(chatId, _users[chatId]);
        }
    }
}