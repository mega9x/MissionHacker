using System.Drawing;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using BitBrowser;
using MissionHacker.ConfigHelper;
using Models;
using Models.BitBrowserApiModels;
using Models.BitBrowserApiModels.Reponse;
using Models.Data;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using SeleniumUndetectedChromeDriver;
using UserAgentGenerator;
using Platform = UserAgentGenerator.Platform;

namespace Crawler;

public class Chrome: IBrowser
{
    private string _remoteUri = "";
    public ChromeDriver Driver { get; private set; } = null;
    public string Proxy { get; set; } = Config.Instance.General!.Proxy;
    public IPData IPData { get; set; } = new();

    /// <summary>
    /// 切换 Ip. 参数是 国家简写
    /// </summary>
    /// <param name="country"></param>
    public IBrowser ChangeIp(string country)
    {
        var uri = Config.Instance.General!.ProxyApi.Replace(Config.Instance.General!.ApiReplacement, country);
        var client = new HttpClient();
        client.GetStringAsync(uri);
        return this;
    }
    public async Task<IWebDriver> GetDriver()
    {
        if (Driver is not null) return Driver;

        if (_remoteUri.Length <= 0)
        {
            var result = await Api.OpenBrowser(Config.Instance.General.BitBrowserId, Config.Instance.General.BitApi);
            _remoteUri = result.Data.Http;
        }
        var chromeOption = new ChromeOptions();
        chromeOption.AddArgument("--incognito");
        chromeOption.AddArgument("--private");
        chromeOption.AddArgument("--lang=en");
        chromeOption.DebuggerAddress = _remoteUri;
        Driver = new(".\\driver", chromeOption);
        Driver.Manage().Window.Size = new Size(843, 680);
        Driver.Manage().Cookies.DeleteAllCookies();
        Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(60);
        Driver.SwitchTo().NewWindow(WindowType.Tab);
        Driver.Url = "https://ip-api.com/";
        var output = Driver.FindElement(By.CssSelector("#codeOutput")).FindElements(By.CssSelector(".string"));
        IPData.Region = output[6].Text;
        IPData.RegionName = output[7].Text;
        IPData.City = output[8].Text;
        IPData.Timezone = output[11].Text;
        IPData.CurrentZip = output[10].Text;
        return Driver;
    }
    public IPData GetIPData()
    {
        return IPData;
    }
    public IBrowser SendKeysByCss(string css, string keys)
    {
        Driver.FindElement(By.CssSelector(css))
            .SendKeys(keys);
        return this;
    }
    public IBrowser ClickByCss(string css)
    {
        Driver.FindElement(By.CssSelector(css)).Click();
        return this;
    }
    public IWebElement QuerySelector(string css)
    {
        return Driver.FindElement(By.CssSelector(css));
    }
}