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
        if (index <= AllMail.Count)
        {
            return "";
        }
        var str = AllMail[index];
        return str;
    }
}