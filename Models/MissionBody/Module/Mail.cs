namespace Models;

public class Mail
{
    public string MailName { get; set; }
    public string MailPd { get; set; }

    public Mail(string str)
    {
        var split = str.Split("|");
        MailName = split[0].Replace("邮箱: ", "").Trim();
        MailPd = split[1].Replace("密码: ", "").Trim();
    }
}