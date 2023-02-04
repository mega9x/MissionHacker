using System.Text.RegularExpressions;
using ConstStr;
using DataLibs;
using OpenQA.Selenium.DevTools.V85.DOMDebugger;

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
    public string Fullname { get; set; } = "";
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
    public async Task<MissionModel> NextCode()
    {
        if (_lock)
        {
            return this;
        }
        if (CodesIndex + 1 >= Codes.Count)
        {
            Time++;
            Libs.Instance.AddTimes(Keyword, 1);
            await Libs.Instance.Save();
            if (Time > MaxTimes)
            {
                _lock = true;
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
        if (File.ReadAllLines(ConfigPath.ManualBlockList).Contains(code))
        {
            return this;
        }
        Codes.Add(new()
        {
            Code = code,
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
    public bool IsAllHandled() => _lock;
    public bool CodesEmpty() => Codes.Count <= 0;
    public Codes? GetCode() => !(_lock || CodesEmpty()) ? Codes[CodesIndex] : null;
}