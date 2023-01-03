namespace Models;

public class MissionModel
{
    // 当指针到了最右侧, 在第二次请求移动指针时锁定.
    // 锁定后, 将会返回空字符串
    private bool _linkLock = false;
    private bool _codeLock = false;
    private List<string> Links { get; set; } = new();
    private List<string> Codes { get; set; } = new();
    private int CodesIndex = 0;
    private int LinksIndex = 0;
    
    public string Area { get; set; } = "";
    public int Times = 0;
    public string Keyword { get; set; } = "";
    public string Platform { get; set; } = "";  
    public MissionModel PrevLink()
    {
        if (LinksIndex <= 0)
        {
            return this;
        }
        _linkLock = false;
        LinksIndex--;
        return this;
    }
    public MissionModel NextLink()
    {
        if (LinksIndex >= Links.Count)
        {
            _linkLock = true;
            return this;
        }
        LinksIndex++;
        return this;
    }
    public MissionModel PrevCode()
    {
        if (CodesIndex <= 0)
        {
            return this;
        }

        _codeLock = false;
        CodesIndex--;
        return this;
    }
    public MissionModel NextCode()
    {
        if (CodesIndex >= Links.Count)
        {
            _codeLock = true;
            return this;
        }
        CodesIndex++;
        return this;
    }

    public MissionModel PushLink(string link)
    {
        Links.Add(link);
        return this;
    }
    public MissionModel PushCode(string code)
    {
        Codes.Add(code);
        return this;
    }
    
    public string GetLink() => _linkLock ? Links[LinksIndex] : "";
    public string GetCodes() => _codeLock ? Codes[CodesIndex] : "";
}