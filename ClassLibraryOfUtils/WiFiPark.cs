using System.Text.Json.Serialization;

namespace ClassLibraryOfUtils;
public class WiFiPark
{
    [JsonPropertyName("ID")] 
    public int Id { get; set; } = 0;

    [JsonPropertyName("global_id")] 
    public long GlobalId { get; init; } = 0;
    
    public string? Name { get; init; }
    public string? AdmArea { get; init; }
    public string? District { get; init; }
    public string? ParkName { get; init; }
    public string? WiFiName { get; init; }
    public int CoverageArea { get; init; } = 0;
    public string? FunctionFlag { get; init; }
    public string? AccessFlag { get; init; }
    public string? Password { get; init; }

    [JsonPropertyName("Longitude_WGS84")] 
    public double LongitudeWgs84 { get; init; } = 0;

    [JsonPropertyName("Latitude_WGS84")]
    public double LatitudeWgs84 { get; init; } = 0;
    [JsonPropertyName("geodata_center")]
    public string? GeoDataCenter { get; init; }

    [JsonPropertyName("geoarea")]
    public string? GeoArea { get; init; }

    public WiFiPark(){}
    public WiFiPark(int id, long globalId, string? name, string? admArea, string? district, string? parkName,
        string? wiFiName, int coverageArea, string? functionFlag, string? accessFlag, string? password,
        double longitudeWgs84, double latitudeWgs84, string? geoDataCenter, string? geoArea)
    {
        Id = id;
        GlobalId = globalId;
        Name = name;
        AdmArea = admArea;
        District = district;
        ParkName = parkName;
        WiFiName = wiFiName;
        CoverageArea = coverageArea;
        FunctionFlag = functionFlag;
        AccessFlag = accessFlag;
        Password = password;
        LongitudeWgs84 = longitudeWgs84;
        LatitudeWgs84 = latitudeWgs84;
        GeoDataCenter = geoDataCenter;
        GeoArea = geoArea;
    }
    
    /// <summary>
    /// The method convert WiFiPark object to csv.
    /// </summary>
    public string ToCsv()
    {
        var properties = GetType().GetProperties();
        string[] result = new string[properties.Length];
        for (int i =0; i < properties.Length; i++)
        {
            var value =properties[i].GetValue(this)?.ToString();
            if (value != null)
            {
                result[i] = value.Replace("\"", "\"\"");

                result[i] = "\"" + value + "\"";
            }
        }

        return String.Join(';', result);
    }

    /// <summary>
    /// The method return an empty list of WiFiPark's objects.
    /// </summary>
    public static List<WiFiPark> Empty()
    {
        return new List<WiFiPark>();
    }

    /// <summary>
    /// The method checks if there are variables equal to null in the object.
    /// </summary>
    /// <returns>true if yes. false else.</returns>
    public bool CheckForNull()
    {
        if (Name is null || AdmArea is null || District is null ||
            ParkName is null || WiFiName is null || FunctionFlag is null ||
            AccessFlag is null || Password is null || GeoArea is null ||
            GeoDataCenter is null)
        {
            return true;
        }

        return false;
    }
}
