using System.Text;

namespace ClassLibraryOfUtils;

/// <summary>
/// The class is used to read and write a collection
/// of WiFiPark objects in CSV format. 
/// </summary>
public class CsvProcesses
{
    public CsvProcesses(){}
    /// <summary>
    /// The method retrieves a list of WiFiPark objects from the stream.
    /// </summary>
    /// <returns>null if the file contain incorrect format data.</returns>
    internal List<WiFiPark>? Read(MemoryStream stream)
    {
        // Read content from the stream.
        string content = Encoding.UTF8.GetString(stream.ToArray());
        string[] lines = content.Split('\n');
        
        // Remove empty strings.
        lines = lines.Where(x => !String.IsNullOrEmpty(x)).ToArray();
        
        if (!CheckFormats.CheckFormat(lines)) return null;
        return ConvertTo.ConvertToListOfWiFiParks(lines);
    }

    /// <summary>
    /// The method writes a CSV representation
    /// of the list of WiFiPark objects to MemoryStream.
    /// </summary>
    public async Task<MemoryStream> Write(List<WiFiPark> wiFiParks)
    {
        // Create the stream.
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        await writer.WriteLineAsync(ConvertTo.ConvertToCsv(FileHeader.HeadInEng));
        await writer.WriteLineAsync(ConvertTo.ConvertToCsv(FileHeader.HeadInRus));
        foreach (var wfp in wiFiParks)
        {
            await writer.WriteLineAsync(wfp.ToCsv());
        }
        // Clear the buffers.
        await writer.FlushAsync();
        stream.Position = 0;
        return stream;
    }
}