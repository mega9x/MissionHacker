using ConstStr;
using Events;

namespace DataLibs;

public class Libs
{

    public static readonly Libs Instance = new Lazy<Libs>(() => new Libs()).Value;
    private Dictionary<string, int> TodayBlock = new();
    private string BlockListPath = ConfigPath.BlockList.Replace("date", DateTime.Now.ToString("yyyy-M-d"));
    
    public Libs()
    {
        MissionEvents.NextCodeEvent += (obj, e) => {
            AddTimes(e.Keyword, 1);
        };
        if (!File.Exists(BlockListPath))
        {
            File.Create(BlockListPath).Close();
        }
        TodayBlock = File.ReadAllLines(BlockListPath)
            .ToDictionary(x => x.Split("|")[0].Trim(), x => int.Parse(x.Split("|")[1].Trim()));
    }
    public int GetCountByKeyword(string keyword)
    {
        if (TodayBlock.ContainsKey(keyword)) return TodayBlock[keyword];
        TodayBlock.Add(keyword, 0);
        return 0;
    }
    public async Task<Libs> Save()
    {
        var list = new List<string>();
        foreach (var (k, v) in TodayBlock)
        {
            list.Add($"{k} | {v}");
        }
        File.Delete(BlockListPath);
        await File.WriteAllLinesAsync(BlockListPath, list);
        return this;
    }
    public Libs AddTimes(string keyword, int times)
    {
        TodayBlock[keyword] += times;
        return this;
    }

    public Libs ClearTodayFinished()
    {
        File.Delete(BlockListPath);
        File.Create(BlockListPath).Close();
        TodayBlock = new();
        return this;
    }
}