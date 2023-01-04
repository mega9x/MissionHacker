using OpenQA.Selenium;

namespace Models;

public class MissionInfo
{
    public Mail? Mail { get; set; }
    public Codes? Codes { get; set; }
    public Links? Links { get; set; }
    public IWebDriver MainDriver { get; set; }
    public IWebDriver MailDriver { get; set; }
}