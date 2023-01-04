using System.Text;
using MissionHacker.ConfigHelper;
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
    public ChromeDriver Driver { get; private set; } = null;
    public string proxy { get; set; } = Config.Instance.General!.Proxy;

    public async Task<IWebDriver> GetDriver(string mailAccount, string mailPd)
    {
        if (Driver is null)
        {
            var chromeOption = new ChromeOptions();
            chromeOption.AddArgument("--incognito");
            chromeOption.AddArgument("--private");
            chromeOption.AddArgument("--lang=en");
            Driver = new ChromeDriver(".\\driver", chromeOption);
            Driver.Manage().Cookies.DeleteAllCookies();
            Driver.Url = "https://outlook.live.com/owa/?cobrandid=ab0455a0-8d03-46b9-b18b-df2f57b9e44c&nlp=1&deeplink=owa/?realm=outlook.com";
            Driver.FindElement(By.Id("i0116")).SendKeys(mailAccount);
            Driver.FindElement(By.Id("idSIButton9")).Click();
            Driver.FindElement(By.Id("i0118")).SendKeys(mailPd);
            Driver.FindElement(By.Id("idSIButton9")).Click();
            try
            {
                Driver.FindElement(By.Id("idBtn_Back")).Click();
            }
            catch
            {
                // ignore
            }
        }
        return this.Driver;
    }

    public async Task<MailChrome> ClickLatest(string keyword)
    {
        if (DriverLock)
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
}