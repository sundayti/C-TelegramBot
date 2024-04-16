namespace ClassLibraryOfUtils;
public static class CsvDecoding
{
    /// <summary>
    /// The method partially decodes a string from the csv format.
    /// Partial decoding is needed to subsequently check the correspondence
    /// of the string format to the csv format.
    /// </summary>
    /// <param name="line"></param>
    /// <returns></returns>
    internal static string?[] PartialCsvDecoding(string[] line)
    {
        string?[] result = { };

        string? element = null;

        for (int i = 0; i < line.Length; i++)
        {
            if (element == null)
            {
                element = line[i].TrimStart();
            }
            else
            {
                element += ";" + line[i];
            }
            /*
             * According to the CSV encoding rules: if a field begins with double-quotes,
             * then it must have an even number of double-quotes
             * (one opening double-quote, one closing double-quote, and all
             * double-quotes inside are duplicated).
             *
             * If a field does not begin with double quotes, then it cannot contain ;.
             *
             * If the program reaches checks i == line.Length - 1, then in
             * file has a departure from the CSV style. This deviation will appear
             * during subsequent checks.
             * In fact, this can happen if the field contains \n, but within the
             * framework of this task this is also a violation of style. 
             */
            if ((element.StartsWith('\"') && element.Count(x => x == '\"') % 2 == 0)
                || !element.StartsWith('\"') || i == line.Length - 1)
            {
                result = result.Append(element.Trim()).ToArray();
                element = null;
            }
        }

        return result;
    }

    /// <summary>
    /// The method completely decodes a string from a CSV format.
    /// </summary>
    /// <param name="line"></param>
    /// <returns>Array of strings in which each element is
    /// a separate field of the original string.</returns>
    internal static string?[] FullCsvDecoding(string line)
    {
        string?[] result = PartialCsvDecoding(line.Split(';'));

        for (int i = 0; i < result.Length; i++)
        {
            if (result[i].Length >= 2)
            {
                if (result[i].StartsWith('\"'))
                {
                    result[i] = result[i][1..^1];
                    result[i] = result[i].Replace("\"\"", "\"");
                }
            }
        }
        return result;
    }
}