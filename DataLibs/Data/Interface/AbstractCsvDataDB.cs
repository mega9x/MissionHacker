using System.Security.Cryptography;
using Utils.Text.Csv;

namespace DataLibs;

public abstract class AbstractDataDB<T> : IDataDB<T>
{
    public AbstractDataDB(string path)
    {
        Path = path;
        this.Data = CsvUtils.ReadListFromCsv<T>(path);
    }
    public string Path { get; }
    public List<T> Data { get; init; }
    public virtual T GetRandom()
    {
        return Data[RandomNumberGenerator.GetInt32(0, Data.Count)];
    }
}