using System.Reflection.Metadata;
using System.Text;
using System.Xml.Linq;
using MissionHacker.ConfigHelper;
using Models;
using Models.BitBrowserApiModels;
using Models.BitBrowserApiModels.Reponse;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools.V106.SystemInfo;
using OpenQA.Selenium.DevTools.V108.Browser;

namespace Crawler;

public class MailChrome
{
    public bool DriverLock { get; private set; } = false;
    private ChromeDriver Driver { get; set; } = null;
    public string proxy { get; set; } = Config.Instance.General!.Proxy;
    public IWebElement InboxBtn { get; set; }
    public IWebElement TrashBtn { get; set; }
    public IWebElement FocusedBtn { get; set; }
    public IWebElement OtherBtn { get; set; }
    public async Task<IWebDriver> GetDriver(Mail mail)
    {
        if (Driver is null)
        {
            var chromeOption = new ChromeOptions();
            chromeOption.AddArgument("--incognito");
            chromeOption.AddArgument("--private");
            chromeOption.AddArgument("--lang=en");
            Driver = new ChromeDriver(".\\driver\\108", chromeOption);
            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(60);
            Driver.Manage().Cookies.DeleteAllCookies();
            Driver.Url = "https://outlook.live.com/owa/?cobrandid=ab0455a0-8d03-46b9-b18b-df2f57b9e44c&nlp=1&deeplink=owa/?realm=outlook.com";
            Driver.FindElement(By.Id("i0116")).SendKeys(mail.MailName);
            await Task.Delay(500);
            Driver.FindElement(By.Id("idSIButton9")).Click();
            Driver.FindElement(By.Id("i0118")).SendKeys(mail.MailPd);
            await Task.Delay(3000);
            Driver.FindElement(By.Id("idSIButton9")).Click();
            try
            {
                await Task.Delay(500);
                Driver.FindElement(By.Id("idBtn_Back")).Click();
            }
            catch
            {
                // ignore
            }
        }
        InboxBtn = Driver.FindElement(By.XPath("/html/body/div[2]/div/div[2]/div[2]/div[2]/div[1]/div/div/div[1]/div/div/div[1]/div/div[1]/div[2]/div"));
        TrashBtn = Driver.FindElement(By.XPath("/html/body/div[2]/div/div[2]/div[2]/div[2]/div[1]/div/div/div[1]/div/div/div[1]/div/div[3]/div[2]/div"));
        FocusedBtn = Driver.FindElement(By.XPath("/html/body/div[2]/div/div[2]/div[2]/div[2]/div[1]/div/div/div[3]/div/div[3]/div[1]/div[1]/div[1]/div/div[2]/div/div/div[1]/button[1]/span/div/div/span/span"));
        OtherBtn = Driver.FindElement(By.XPath("/html/body/div[2]/div/div[2]/div[2]/div[2]/div[1]/div/div/div[3]/div/div[3]/div[1]/div[1]/div[1]/div/div[2]/div/div/div[1]/button[2]/span/div/div/span/span"));
        return this.Driver;
    }

    public async Task<MailChrome> ClickLatestFocused(string keyword)
    {
        InboxBtn.Click();
        FocusedBtn.Click();
        if (DriverLock || Driver is null)
        {
            return this;
        }

        DriverLock = true;
        var timeout = 0;
        while (timeout <= 60)
        {
            await Task.Delay(1000);
            timeout++;
            IWebElement selected;
            try
            {
                selected = Driver.FindElement(By.CssSelector(".hcptT"));
            }
            catch
            {
                continue;
            }
            if (!selected.GetAttribute("area-label").Contains("keyword")) continue;
            selected.Click();
            break;
        }
        DriverLock = false;
        return this;
    }

    public async Task<MailChrome> ClickLatestOther(string keyword)
    {
        InboxBtn.Click();
        OtherBtn.Click();
        if (DriverLock || Driver is null)
        {
            return this;
        }
       
        DriverLock = true;
        var timeout = 0;
        while (timeout <= 60)
        {
            await Task.Delay(1000);
            timeout++;
            IWebElement selected;
            try
            {
                selected = Driver.FindElement(By.CssSelector(".hcptT"));
            }
            catch
            {
                continue;
            }
            if (!selected.GetAttribute("area-label").Contains("keyword")) continue;
            selected.Click();
            break;
        }
        DriverLock = false;
        return this;
    }
    public MailChrome SendKeysByCss(string css, string keys)
    {
        Driver.FindElement(By.CssSelector(css))
            .SendKeys(keys);
        return this;
    }
    public MailChrome ClickByCss(string css)
    {
        Driver.FindElement(By.CssSelector(css)).Click();
        return this;
    }
    public IWebElement QuerySelector(string css)
    {
        return Driver.FindElement(By.CssSelector(css));
    }
}