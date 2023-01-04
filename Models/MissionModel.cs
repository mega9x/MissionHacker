namespace Models;

public class MissionModel
{
    // 当指针到了最右侧, 在第二次请求移动指针时锁定.
    // 锁定后, 将会返回空字符串
    private bool _linkLock = false;
    private bool _codeLock = false;
    private List<Links> Links { get; set; } = new();
    private List<Codes> Codes { get; set; } = new();
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

    public MissionModel PushLink(string link, string domain)
    {
        Links.Add(new()
        {
            Link = link,
            Domain = domain
        });
        return this;
    }
    public MissionModel PushCode(string code, string domain)
    {
        Codes.Add(new ()
        {
            Code = code,
            Domain = domain
        });
        return this;
    }

    public bool IsLinksAllHandled() => _linkLock;
    public bool IsCodesAllHandled() => _codeLock;
    public Links? GetLink() => _linkLock ? Links[LinksIndex] : null;
    public Codes? GetCodes() => _codeLock ? Codes[CodesIndex] : null;
}