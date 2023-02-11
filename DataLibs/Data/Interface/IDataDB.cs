namespace DataLibs;

public interface IDataDB<T>
{
    string Path { get; }
    List<T> Data { get; init; }
    T GetRandom();
}