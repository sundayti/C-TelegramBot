namespace ClassLibraryOfUtils;
/// <summary>
/// The class storing data of a particular user
/// </summary>
public class UserData
{
    public List<WiFiPark>? WiFiParks { get; set; }
    public bool IsSampleByCoverageArea { get; set; }
    public bool IsSampleByParkName { get; set; }
    public bool IsSampleByAdmArea { get; set; }
    public List<string> Coverages { get; set; } = new();
    public List<string> ParkNames { get; set; } = new();
    public List<string> AdmAreas { get; set; } = new();
    public string Text { get; set; } = "";

    public UserData()
    {
        WiFiParks = null;
        ToDefault();
    }

    public void ToDefault()
    {
        IsSampleByCoverageArea = false;
        IsSampleByParkName = false;
        IsSampleByAdmArea = false;
    }
}

