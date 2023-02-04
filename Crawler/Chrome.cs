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

public class Chrome : AbstractBrowser
{
    private string _remoteUri = "";
    public override async Task<IWebDriver> GetDriver()
    {
        if (Driver is not null) return Driver;
        if (_remoteUri.Length <= 0)
        {
            var result = await Api.OpenBrowser(Config.Instance.MissionHackerConfig.General.BitBrowserId, Config.Instance.MissionHackerConfig.General.BitApi);
            _remoteUri = result.Data.Http;
        }
        RemoteUri = _remoteUri;
        InitChrome();
        Driver.Manage().Window.Size = new Size(990 + RandomNumberGenerator.GetInt32(0, 401), 800 + RandomNumberGenerator.GetInt32(0, 201));
        RefreshIpData();
        return Driver;
    }
}