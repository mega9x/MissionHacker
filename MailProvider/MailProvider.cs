

using ConstStr;

namespace MailProvider;

public class MailProvider
{
    public static MailProvider Instance = new Lazy<MailProvider>(() => new MailProvider()).Value;
    private List<string> AllMail { get; set; } = new();
    private int index = 0;
    public string CurrentMail => AllMail[index - 1];
    public async Task<string> GetMail()
    {
        var mAllLinesAsync = await File.ReadAllLinesAsync(ConfigPath.MailPool);
        AllMail = mAllLinesAsync.ToList();
        var str = AllMail[index];
        index++;
        return str;
    }
    public MailProvider Record(string keyword)
    {
        var str = $"{CurrentMail} | {keyword}{Environment.NewLine}";
        File.AppendAllText(ConfigPath.ConfigMailUsed, str);
        return this;
    }
    public bool IsMailUsed(string keyword)
    {
        var used = File.ReadAllLines(ConfigPath.ConfigMailUsed);
        var str = $"{CurrentMail} | {keyword}";
        return used.Contains(str);
    }
}