using Models.ConstStr;

namespace MailProvider;

public class MailProvider
{
    private List<string> AllMail { get; set; }
    private int index = 0;
    public MailProvider()
    {
        AllMail = File.ReadAllLines(ConfigPath.MAIL_POOL).ToList();
    }
    public string GetMail()
    {
        var str = "";
        while (true)
        {
            index++;
            if (index >= AllMail.Count)
            {
                break;
            }
            str = AllMail[index--];
            if (File.ReadAllLines(ConfigPath.CONFIG_MAIL_USED).Contains(str))
            {
                continue;
            }
            File.AppendAllText(ConfigPath.CONFIG_MAIL_USED,  $"{AllMail[index]}{Environment.NewLine}");
            break;
        }
        return str;
    }
}