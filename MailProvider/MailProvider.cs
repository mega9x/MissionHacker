using Models.ConstStr;

namespace MailProvider;

public class MailProvider
{
    public static MailProvider Instance = new Lazy<MailProvider>(() => new MailProvider()).Value;
    private List<string> AllMail { get; set; } = new();
    private int index = 0;
    public string CurrentMail => AllMail[index];
    public async Task<string> GetMail()
    {
        if (AllMail.Count <= 0)
        {
            var mAllLinesAsync = await File.ReadAllLinesAsync(ConfigPath.MAIL_POOL);
            AllMail = mAllLinesAsync.ToList();
        }
        var str = AllMail[index];
        index++;
        return str;
    }
    public MailProvider Record(string keyword)
    {
        var str = $"{AllMail[index - 1]} | {keyword}{Environment.NewLine}";
        File.AppendAllText(ConfigPath.CONFIG_MAIL_USED, str);
        return this;
    }
    public bool IsMailUsed(string keyword)
    {
        var used = File.ReadAllLines(ConfigPath.CONFIG_MAIL_USED);
        var str = $"{AllMail[index - 1]} | {keyword}";
        return used.Contains(str);
    }
}