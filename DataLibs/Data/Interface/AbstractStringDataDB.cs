using System.Security.Cryptography;
using Utils.Text.Csv;

namespace DataLibs;

public class AbstractStringDataDB: IDataDB<string>
{
    public AbstractStringDataDB(string path)
    {
        Path = path;
        Data = File.ReadAllLines(path).ToList();
    }
    public string Path { get; }
    public List<string> Data { get; init; }
    public virtual string GetRandom()
    {
        return Data[RandomNumberGenerator.GetInt32(0, Data.Count)];
    }
}