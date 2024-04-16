namespace ClassLibraryOfUtils;

public static class ConvertTo
{
    /// <summary>
    /// The method convert array of strings to headquarters class array. 
    /// </summary>
    /// <param name="content">Array of strings to convert.</param>
    /// <returns>Headquarters class array.</returns>
    internal static List<WiFiPark>? ConvertToListOfWiFiParks(string[] content)
    {
        List<WiFiPark> wiFiParks = new List<WiFiPark>();
        // Divide the table into columns.
        Dictionary<string, List<string?>> fields = ConvertToDictionary(content);

        // Program create headquarters based on the data received.
        try
        {
            for (int i = 0; i < content.Length - 2; i++)
            {
                WiFiPark wfp = new WiFiPark(
                    id: int.Parse(fields["ID"][i]),
                    globalId: long.Parse(fields["global_id"][i]),
                    name: fields["Name"][i],
                    admArea: fields["AdmArea"][i],
                    district: fields["District"][i],
                    parkName: fields["ParkName"][i],
                    wiFiName: fields["WiFiName"][i],
                    coverageArea: int.Parse(fields["CoverageArea"][i]),
                    functionFlag: fields["FunctionFlag"][i],
                    accessFlag: fields["AccessFlag"][i],
                    password: fields["Password"][i],
                    longitudeWgs84: double.Parse(fields["Longitude_WGS84"][i].Replace('.', ',')),
                    latitudeWgs84: double.Parse(fields["Latitude_WGS84"][i].Replace('.', ',')),
                    geoDataCenter: fields["geodata_center"][i],
                    geoArea: fields["geoarea"][i]);
                // Add the element to list of headquarters.
                wiFiParks.Add(wfp);
            }
        }
        catch (AggregateException) { return null; }
        catch (FormatException) { return null; }

        return wiFiParks;
    }
    
    /// <summary>
    /// The method creates a dictionary whose members are the
    /// columns of the passed table. 
    /// </summary>
    /// <returns>The dictionary, where the keys is the column names
    /// and the value is an list of strings, in which the line number i
    /// corresponds to the value of the column in row number i+1.</returns>
    static Dictionary<string, List<string?>> ConvertToDictionary(string[] content)
    {
        Dictionary<string, List<string?>> dictionary = new Dictionary<string, List<string?>>();

        // Completely decodes the content from the CSV format
        string?[][] table = ConvertToArrayOfArrays(content);

        for (int i = 0; i < table.Length; i++)
        {
            for (int j = 0; j < table[i].Length; j++)
            {
                // The first line in table is header. Creating keys by headings.
                if (i == 0)
                {
                    dictionary.Add(table[0][j], new List<string?>());
                    continue;
                }
                if (i == 1) continue;

                // Subsequent lines contain the values. Add them to the values.
                dictionary[table[0][j]].Add(table[i][j]);
            }
        }

        return dictionary;
    }

    /// <summary>
    /// The method completely decodes the table from the CSV format.
    /// </summary>
    /// <param name="arrayStrings"></param>
    /// <returns></returns>
    static string?[][] ConvertToArrayOfArrays(string[] arrayStrings)
    {
        string?[][] result = new string?[arrayStrings.Length][];

        for (int i = 0; i != arrayStrings.Length; i++)
        {
            result[i] = CsvDecoding.FullCsvDecoding(arrayStrings[i])[..FileHeader.HeadInEng.Length];
        }

        return result;
    }
    
    /// <summary>
    /// The method combines values from an array of
    /// strings according to RFC 4180 standards.
    /// </summary>
    /// <param name="content">Array of string to be encoded.</param>
    /// <returns>CSV format string.</returns>
    internal static string ConvertToCsv(string[] content)
    {
        string[] result = new string[content.Length];
        for (int i = 0; i < content.Length; i++)
        {
            result[i] = content[i].Replace("\"", "\"\"");

            result[i] = "\"" + content[i] + "\"";
        }

        return String.Join(';', result);
    }
}