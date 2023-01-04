using System.Drawing;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using MissionHacker.ConfigHelper;
using Models;
using Models.BitBrowserApiModels;
using Models.BitBrowserApiModels.Reponse;
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
    public string proxy { get; set; } = Config.Instance.General!.Proxy;
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
        if (Driver is null)
        {
            if (_remoteUri.Length <= 0)
            {
                await OpenBrowser();
            }
            var chromeOption = new ChromeOptions();
            chromeOption.AddArgument("--incognito");
            chromeOption.AddArgument("--private");
            chromeOption.AddArgument("--lang=en");
            chromeOption.DebuggerAddress = _remoteUri;
            Driver = new ChromeDriver(".\\driver", chromeOption);
            Driver.Manage().Window.Size = new Size(843, 680);
            Driver.Manage().Cookies.DeleteAllCookies();
        }
        return this.Driver;
    }

    private async Task OpenBrowser()
    {
        var httpClient = new HttpClient();
        var browserPara = new OpenBrowser()
        {
            Id = Config.Instance.General!.BitBrowserId,
        };
        var data = new StringContent(JsonSerializer.Serialize(browserPara), Encoding.UTF8, "application/json");
        var response = await httpClient.PostAsync(Config.Instance.General.BitApi + "/browser/open", data);
        var resStr = await response.Content.ReadAsStringAsync();
        Console.WriteLine(resStr);
        var resObj = JsonSerializer.Deserialize<ResponseBase<OpenBrowserResponse>>(resStr);
        if (resObj is null)
        {
            throw new Exception("Failed to open browser");
        }
        _remoteUri = resObj.Data.Http;
    }
}