using System.Drawing;
using System.Runtime.CompilerServices;
using MissionHacker.ConfigHelper;
using OpenQA.Selenium.Chrome;
using SeleniumUndetectedChromeDriver;

namespace Crawler;

public class Chrome
{
    public ChromeDriver Driver { get; private set; }
    public string proxy { get; set; } = Config.Instance.General!.Proxy;
    public Chrome()
    {
        var chromeOption = new ChromeOptions();
        chromeOption.AddArgument($"--proxy-server=socks5://{proxy}");
        chromeOption.AddArgument("--incognito");
        chromeOption.AddArgument("--user-agent=Mozilla/6.0 (Windows NT 6.3; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/104.0.5112.102 Safari/537.36");
        chromeOption.AddArgument("--private");
        chromeOption.AddArgument("--lang=en");
        Driver = UndetectedChromeDriver.Create(chromeOption, driverExecutablePath: "./Drivers/chromedriver.exe");
        Driver.Manage().Window.Size = new Size(843, 680);
        Driver.Manage().Cookies.DeleteAllCookies();
    }
    /// <summary>
    /// 切换 Ip. 参数是 国家简写
    /// </summary>
    /// <param name="country"></param>
    public void ChangeIp(string country)
    {
        var uri = Config.Instance.General!.ProxyApi.Replace(Config.Instance.General!.ApiReplacement, country);
        var client = new HttpClient();
        client.GetStringAsync(uri);
    }
}