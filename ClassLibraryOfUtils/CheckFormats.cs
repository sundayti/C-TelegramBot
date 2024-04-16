using static ClassLibraryOfUtils.CsvDecoding;
using static ClassLibraryOfUtils.FileHeader;
namespace ClassLibraryOfUtils;

public static class CheckFormats
{
    /// <summary>
    /// The method checks whether the data format
    /// in the file meets the specified requirements.
    /// </summary>
    /// <param name="table"></param>
    /// <returns>true - the data format in the file
    /// does meet the requirements.
    /// false - the data format in the file does not
    /// meet the requirements.
    /// </returns>
    internal static bool CheckFormat(string[] table)
    {
        // The file must have one header lines.
        if (!(table.Length >= 1))
        {
            return false;
        }

        // Checking each line of the file for compliance with the format.
        foreach (var line in table)
        {
            string?[] encodingLine = PartialCsvDecoding(line.Split(';'));

            if (!CheckAllFields(encodingLine) || !CheckLength(encodingLine))
            {
                return false;
            }
        }
        
        return CheckHeaders(table[0], table[1]);
    }

    static bool CheckAllFields(string?[] fields)
    {
        // Checking each field in the line.
        foreach (string? el in fields)
        {
            // If the field begins with \" then according to the
            // RFC 4180 standard it must end with \" .
            if (el.StartsWith('\"') && !el.EndsWith('\"'))
            {
                return false;
            }

            // If a field is enclosed in double-quotes, then all
            // enclosed double-quotes are duplicated.
            if ((el.StartsWith('\"') && el.EndsWith('\"') && el != "\"\"") &&
                el.Replace("\"\"", "\"").Count(x => x == '\"') * 2 !=
                el.Count(x => x == '\"') + 2)
            {
                return false;
            }
        }

        return true;
    }

    static bool CheckLength(string?[] encodingLine)
    {
        // Check the number of fields in the line.
        if (encodingLine.Length < HeadInEng.Length)
        {
            return false;
        }

        // Checking that only the first 10 columns are filled.
        for (int i = HeadInEng.Length; i < encodingLine.Length; i++)
        {
            if (encodingLine[i] != "" && encodingLine[i] != "\"\"")
            {
                return false;
            }
        }

        return true;
    }

    static bool CheckHeaders(string headerEng, string headerRus)
    {
        // Check the correctness of the header.
        string?[] firstLine = FullCsvDecoding(headerEng);
        string?[] secondLine = FullCsvDecoding(headerRus);
        firstLine = firstLine[..HeadInEng.Length];
        secondLine = secondLine[..HeadInEng.Length];
        if (!firstLine.SequenceEqual(HeadInEng) || !secondLine.SequenceEqual(HeadInRus))
        {
            return false;
        }

        return true;
    }
}