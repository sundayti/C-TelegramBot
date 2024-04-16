using System.Text.Json;
using System.Text.Unicode;
namespace ClassLibraryOfUtils;

/// <summary>
/// The class is used to read and write a collection
/// of WiFiPark objects in JSON format. 
/// </summary>
public class JsonProcesses
{
    public JsonProcesses(){}
    /// <summary>
    /// The method writes a JSON representation
    /// of the list of WiFiPark objects to MemoryStream.
    /// </summary>
    public async Task<MemoryStream> Write(List<WiFiPark> wiFiParks)
    {
        var options = new JsonWriterOptions
        {
            Indented = true,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(UnicodeRanges.All)
        };
        var stream = new MemoryStream();
        var writer = new Utf8JsonWriter(stream, options);
        JsonSerializer.Serialize(writer, wiFiParks);
        await writer.FlushAsync();
        stream.Position = 0;
        return stream;
    }

    /// <summary>
    /// The method retrieves a list of WiFiPark objects from the stream.
    /// </summary>
    /// <returns>null if the file contain incorrect format data.</returns>
    public List<WiFiPark>? Read(MemoryStream stream)
    {
        try
        {
            List<WiFiPark>? wiFiParks =
                JsonSerializer.Deserialize<List<WiFiPark>>(stream.ToArray());
            foreach (var wfp in wiFiParks ?? WiFiPark.Empty())
            {
                if (wfp.CheckForNull()) return null;
            }
            return wiFiParks?.ToList();
        }catch(JsonException){return null;}
    }
}