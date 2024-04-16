using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace ClassLibraryOfUtils;
public class MenuSender
{
    private readonly ITelegramBotClient _botClient;
    private readonly CancellationToken _cancellationToken;
    public MenuSender(ITelegramBotClient botClient, CancellationToken cancellationToken)
    {
        _botClient = botClient;
        _cancellationToken = cancellationToken;
    }
    public MenuSender(){}
    /// <summary>
    /// The method for sending the main menu.
    /// </summary>
    public async Task SendMainMenu(long chatId, UserData userData)
    {
        string text = "Select the menu item:";
        AddText(ref text, userData.Text);
        
        InlineKeyboardMarkup inlineKeyboardMarkup = new(new[]
        {
            new InlineKeyboardButton[] { "Sampling"!, "Sorting"! },
            new InlineKeyboardButton[] { "Upload a new file"! },
            new InlineKeyboardButton[] { "Download the processed file"! }
        });
        

        await _botClient.SendTextMessageAsync(
            chatId: chatId,
            text: text,
            replyMarkup: inlineKeyboardMarkup,
            cancellationToken: _cancellationToken);
    }

    /// <summary>
    /// A method for sending the SampleMenu.
    /// </summary>
    public async Task SendSampleMenu(long chatId, int messageId, UserData userData)
    {
        string text = "Select the type of filtering:";
        AddText(ref text, userData.Text);

        InlineKeyboardMarkup inlineKeyboardMarkup = new(new[]
        {
            new InlineKeyboardButton[] { "CoverageArea"!, "ParkName"! },
            new InlineKeyboardButton[] { "AdmArea and CoverageArea"! },
            new InlineKeyboardButton[] { "Back"! }
        });
        await _botClient.EditMessageTextAsync(
            chatId: chatId,
            messageId: messageId,
            text: text,
            replyMarkup: inlineKeyboardMarkup,
            cancellationToken: _cancellationToken);
    }

    /// <summary>
    /// A method for sending the SortingMenu.
    /// </summary>
    public async Task SortingMenu(long chatId, int messageId, UserData userData)
    {
        string text = "Select the type of sorting:";
        AddText(ref text, userData.Text);

        InlineKeyboardMarkup inlineKeyboardMarkup = new(new[]
        {
            new InlineKeyboardButton[] { "CoverageArea in ascending order"! },
            new InlineKeyboardButton[] { "Name by alphabetical order"! },
            new InlineKeyboardButton[] { "Back"! }
        });
        await _botClient.EditMessageTextAsync(
            chatId: chatId,
            messageId: messageId,
            text: text,
            replyMarkup: inlineKeyboardMarkup,
            cancellationToken: _cancellationToken);
    }

    /// <summary>
    /// A method for sending the file format select menu.
    /// </summary>
    public async Task AskTypeFile(long chatId, int messageId, UserData userData)
    {
        string text = "Select the file format:";
        AddText(ref text, userData.Text);

        InlineKeyboardMarkup inlineKeyboardMarkup = new(new[]
        {
            new InlineKeyboardButton[] { "JSON"!, "CSV"! },
            new InlineKeyboardButton[] { "Back"! }
        });
        await _botClient.EditMessageTextAsync(
            chatId:chatId, 
            messageId: messageId,
            text: text,
            replyMarkup: inlineKeyboardMarkup,
            cancellationToken: _cancellationToken);
    }

    /// <summary>
    /// The method for sending the main menu.
    /// </summary>
    public async Task SendMainMenu(long chatId, int messageId, UserData userData)
    {
        string text = "Select the menu item:";
        AddText(ref text, userData.Text);

        InlineKeyboardMarkup inlineKeyboardMarkup = new(new[]
        {
            new InlineKeyboardButton[] { "Sampling"!, "Sorting"! },
            new InlineKeyboardButton[] { "Upload a new file"! },
            new InlineKeyboardButton[] { "Download the processed file"! }
        });


        await _botClient.EditMessageTextAsync(
            chatId: chatId,
            messageId: messageId,
            text: text,
            replyMarkup: inlineKeyboardMarkup,
            cancellationToken: _cancellationToken);
    }

    /// <summary>
    /// Method for sending a menu with CoverageArea options.
    /// </summary>
    public async Task SelectCoverageArea(long chatId, int messageId, UserData userData)
    {
        string text = "Select a value for CoverageArea or enter it manually:";
        AddText(ref text, userData.Text);

        List<int>? coverages = userData.WiFiParks?.Select(item => item.CoverageArea).ToList();
        userData.Coverages = new List<string>();
        InlineKeyboardMarkup inlineKeyboardMarkup = CreateMarkup(coverages, userData.Coverages);
        
        await _botClient.EditMessageTextAsync(
            chatId: chatId,
            messageId: messageId,
            text: text,
            replyMarkup: inlineKeyboardMarkup,
            cancellationToken: _cancellationToken);
    }

    /// <summary>
    /// Method for sending a menu with CoverageArea options.
    /// </summary>
    public async Task SelectCoverageArea(long chatId, UserData userData)
    {
        string text = "Select a value for CoverageArea or enter it manually:";
        AddText(ref text, userData.Text);

        List<int>? coverages = userData.WiFiParks?.Select(item => item.CoverageArea).ToList();
        userData.Coverages = new List<string>();
        InlineKeyboardMarkup inlineKeyboardMarkup = CreateMarkup(coverages, userData.Coverages);

        await _botClient.SendTextMessageAsync(
            chatId: chatId,
            text: text,
            replyMarkup: inlineKeyboardMarkup,
            cancellationToken: _cancellationToken);
    }

    /// <summary>
    /// Method for sending a menu with AdmArea options.
    /// </summary>
    public async Task SelectAdmArea(long chatId, int messageId, UserData userData)
    {
        string text = "Select a value for AdmArea or enter it manually:";
        AddText(ref text, userData.Text);

        List<string>? admAreas = userData.WiFiParks?.Select(item => item.AdmArea).ToList();
        userData.AdmAreas = new List<string>();
        InlineKeyboardMarkup inlineKeyboardMarkup = CreateMarkup(admAreas, userData.AdmAreas);

        await _botClient.EditMessageTextAsync(
            chatId: chatId,
            messageId: messageId,
            text: text,
            replyMarkup: inlineKeyboardMarkup,
            cancellationToken: _cancellationToken);
    }

    /// <summary>
    /// Method for sending a menu with ParkName options.
    /// </summary>
    public async Task SelectParkName(long chatId, int messageId, UserData userData)
    {
        string text = "Select a value for ParkName or enter it manually.";
        AddText(ref text, userData.Text);

        List<string>? parkNames = userData.WiFiParks?.Select(item => item.ParkName).ToList();
        userData.ParkNames = new List<string>();
        InlineKeyboardMarkup inlineKeyboardMarkup = CreateMarkup(parkNames, userData.ParkNames);

        await _botClient.EditMessageTextAsync(
            chatId: chatId,
            messageId: messageId,
            text: text,
            replyMarkup: inlineKeyboardMarkup,
            cancellationToken: _cancellationToken);
    }

    /// <summary>
    /// The method creates a markup with unique elements from the list.
    /// </summary>
    private InlineKeyboardMarkup CreateMarkup<T>(List<T>? listOfItems, List<string> userList)
    {
        listOfItems = listOfItems?.Distinct().ToList();
        List<InlineKeyboardButton[]> inlineKeyboardButtons = new List<InlineKeyboardButton[]>();

        for (int i = 0; i < listOfItems?.Count / 2; i++)
        {
            List<InlineKeyboardButton> buttons = new List<InlineKeyboardButton>();
            for (int j = 0; j < 2; j++)
            {
                userList.Add(listOfItems?[i * 2 + j]?.ToString() ?? string.Empty);
                buttons.Add(AddButton(listOfItems?[i * 2 + j]?.ToString(), i * 2 + j));
            }
            inlineKeyboardButtons.Add(buttons.ToArray());
        }

        if (listOfItems?.Count % 2 == 1)
        {
            userList.Add(listOfItems!.Last().ToString());
            inlineKeyboardButtons.Add(new[] 
                { AddButton(listOfItems?.Last()?.ToString(), listOfItems!.Count - 1) });
        }

        if (listOfItems?.Count == 0)
        {
            inlineKeyboardButtons.Add(new[]{InlineKeyboardButton.WithCallbackData("Nothing")});
        }
        InlineKeyboardMarkup inlineKeyboardMarkup = new(inlineKeyboardButtons);
        return inlineKeyboardMarkup;
    }

    private void AddText(ref string  text, string actionsTaken)
    {
        if (actionsTaken != "")
        {
            text = "Actions taken:\n" + actionsTaken + "\n\n" + text;
        }
    }

    private InlineKeyboardButton AddButton(string? data, int index)
    {
        InlineKeyboardButton button;
        if (data?.Length > 18)
        { 
            button = InlineKeyboardButton.WithCallbackData(
                data[..15] + "...",
                $"{index}");
        }
        else
        {
            button = InlineKeyboardButton.WithCallbackData(
                data ?? string.Empty,
                $"{index}");
        }

        return button;
    }
}

