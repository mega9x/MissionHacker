using System.Drawing;
using System.Security.Cryptography;
using System.Text.Json;
using BitBrowser;
using MissionHacker.ConfigHelper;
using Models.Data;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Internal;

namespace Crawler;

public class Chrome: AbstractBrowser
{
    private string _remoteUri = "";
    /// <summary>
    /// 切换 Ip. 参数是 国家简写
    /// </summary>
    /// <param name="country"></param>
    public override IBrowser ChangeIp(string country)
    {
        var uri = Config.Instance.General!.ProxyApi.Replace(Config.Instance.General!.ApiReplacement, country);
        var client = new HttpClient();
        client.GetStringAsync(uri);
        return this;
    }
    public override async Task<IWebDriver> GetDriver()
    {
        if (Driver is not null) return Driver;
        if (_remoteUri.Length <= 0)
        {
            var result = await Api.OpenBrowser(Config.Instance.General.BitBrowserId, Config.Instance.General.BitApi);
            _remoteUri = result.Data.Http;
        }
        RemoteUri = _remoteUri;
        InitChrome();
        Driver.Manage().Window.Size = new Size(843 + RandomNumberGenerator.GetInt32(0, 401), 600 + RandomNumberGenerator.GetInt32(0, 201));
        RefreshIpData();
        return Driver;
    }
}