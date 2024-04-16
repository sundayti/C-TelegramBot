namespace ClassLibraryOfUtils;

/// <summary>
/// Class to perform sampling in the file.
/// </summary>
public class SampleProcesses
{
    private readonly Dictionary<long, UserData> _users;
    private readonly MenuSender _menuSender;
    public SampleProcesses(Dictionary<long, UserData> users, MenuSender menuSender)
    {
        _menuSender = menuSender;
        _users = users;
    }
    public SampleProcesses(){}
    /// <summary>
    /// Method for sampling the list of WiFiParks by AdmArea
    /// </summary>
    async internal Task SampleByAdmArea(long chatId, string text)
    {
        _users[chatId].WiFiParks = _users[chatId].WiFiParks?.Where(x => x.AdmArea == text).ToList();
        _users[chatId].Text += $"\nAdmArea sampling: {text}";
        await _menuSender.SelectCoverageArea(chatId, _users[chatId]);
        _users[chatId].ToDefault();
        _users[chatId].IsSampleByCoverageArea = true;
    }

    /// <summary>
    /// Method for sampling the list of WiFiParks by AdmArea
    /// </summary>
    async internal Task SampleByAdmArea(long chatId, int messageId, int index)
    {
        UserData userData = _users[chatId];
        // Since callback date can't weigh much I had to invent a crutch with a list and an index.
        userData.WiFiParks = userData.WiFiParks?.Where(x => x.AdmArea == userData.AdmAreas[index]).ToList();
        userData.Text += $"\nAdmArea sampling: {userData.AdmAreas[index]}";
        await _menuSender.SelectCoverageArea(chatId, messageId, _users[chatId]);
        userData.ToDefault();
        userData.IsSampleByCoverageArea = true;
    }

    /// <summary>
    /// Method for sampling the list of WiFiParks by AdmArea
    /// </summary>
    async internal Task SampleByParkName(long chatId, string text)
    {
        _users[chatId].WiFiParks = _users[chatId].WiFiParks?.Where(x => x.ParkName == text).ToList();
        _users[chatId].Text += $"\nParkName sampling: {text}";
        await _menuSender.SendMainMenu(chatId, _users[chatId]);
        _users[chatId].ToDefault();
    }

    /// <summary>
    /// Method for sampling the list of WiFiParks by AdmArea
    /// </summary>
    async internal Task SampleByParkName(long chatId, int messageId, int index)
    {
        UserData userData = _users[chatId];
        // Since callback date can't weigh much I had to invent a crutch with a list and an index.
        userData.WiFiParks = userData.WiFiParks?.Where(x => x.ParkName == userData.ParkNames[index]).ToList();
        userData.Text += $"\nParkName sampling: {userData.ParkNames[index]}";
        await _menuSender.SendMainMenu(chatId, messageId, _users[chatId]);
        userData.ToDefault();
    }

    /// <summary>
    /// Method for sampling the list of WiFiParks by AdmArea
    /// </summary>
    async internal Task SampleByCoverageArea(long chatId, string text, SendMessage sender, int messageId)
    {
        if (!int.TryParse(text, out int n))
        {
            await sender.SendReplyMessage(
                "CoverageArea can only accept numeric values.\nTry again.",
                chatId, messageId);
            return;
        }
        _users[chatId].WiFiParks = _users[chatId].WiFiParks?.Where(x => x.CoverageArea == n).ToList();
        _users[chatId].Text += $"\nCoverageArea sampling: {n}";
        await _menuSender.SendMainMenu(chatId, _users[chatId]);
        _users[chatId].ToDefault();
    }

    /// <summary>
    /// Method for sampling the list of WiFiParks by AdmArea
    /// </summary>
    async internal Task SampleByCoverageArea(long chatId,
        int messageId, int index)
    {
        UserData userData = _users[chatId];
        // Since callback date can't weigh much I had to invent a crutch with a list and an index.
        userData.WiFiParks = userData.WiFiParks?.Where(x => x.CoverageArea == int.Parse(userData.Coverages[index])).ToList();
        userData.Text += $"\nCoverageArea sampling: {userData.Coverages[index]}";
        await _menuSender.SendMainMenu(chatId, messageId, _users[chatId]);
        userData.ToDefault();
    }
}