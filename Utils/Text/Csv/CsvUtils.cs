using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

namespace Utils.Text.Csv;

public class CsvUtils
{
    public static T ReadFromCsv<T>(string path)
    {
        using var reader = new StreamReader(path);
        using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.CurrentCulture)
        {
            BadDataFound = null,
            MissingFieldFound = null
        });
        return csv.GetRecord<T>();
    }
    public static List<T> ReadListFromCsv<T>(string path)
    {
        using var reader = new StreamReader(path);
        using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.CurrentCulture)
        {
            BadDataFound = null,
            MissingFieldFound = null
        });
        return csv.GetRecords<T>().ToList();
    }
}