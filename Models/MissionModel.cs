using System.Text.RegularExpressions;
using Models.ConstStr;

namespace Models;

public class MissionModel
{
    // 当指针到了最右侧, 在第二次请求移动指针时锁定.
    // 锁定后, 将会返回空字符串
    private bool _lock = false;
    private int CodesIndex = 0;
    public int Time = 1;
    private List<Links> Links { get; } = new();
    private List<Codes> Codes { get; } = new();
    public string Area { get; set; } = "";
    public int MaxTimes { get; set; } = 1;
    public string Keyword { get; set; } = "";
    public string Platform { get; set; } = "";

    public MissionModel PrevCode()
    {
        if (CodesIndex <= 0)
        {
            return this;
        }
        CodesIndex--;
        return this;
    }
    public MissionModel NextCode()
    {
        if (_lock)
        {
            return this;
        }
        if (CodesIndex >= Codes.Count)
        {
            Time++;
            if (Time > MaxTimes)
            {
                _lock = true;
                AddAllBlockList();
                return this;
            }
            CodesIndex = 0;
            return this;
        }
        CodesIndex++;
        return this;
    }
    public MissionModel PushCode(string code, string domain)
    {
        var uriRegex = new Regex("(https?|ftp|file)://[-A-Za-z0-9+&@#/%?=~_|!:,.;]+[-A-Za-z0-9+&@#/%=~_|]");
        var strLink = uriRegex.Match(code).Value;
        if (File.ReadAllLines(ConfigPath.MANUAL_BLOCK_LSIT).Contains(strLink))
        {
            return this;
        }
        Codes.Add(new ()
        {
            Code = strLink,
            Domain = domain,
        });
        return this;
    }
    public MissionModel Lock()
    {
        _lock = true;
        return this;
    }
    public MissionModel UnLock()
    {
        _lock = false;
        return this;
    }
    public MissionModel AddAllBlockList()
    {
        using StreamWriter writer = new(File.OpenWrite(ConfigPath.BLOCK_LSIT.Replace("date", DateTime.Now.ToString("yyyy-M-d"))));
        writer.WriteLine(Keyword);
        return this;
    }
    public bool IsAllHandled() => _lock;
    public bool CodesEmpty() => Codes.Count <= 0;
    public Codes? GetCodes() => !_lock && !CodesEmpty() ? Codes[CodesIndex] : null;
}