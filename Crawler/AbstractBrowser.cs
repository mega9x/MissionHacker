using MissionHacker.ConfigHelper;
using Models.Data;
using System.Text;
using System.Text.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;
using UserAgentGenerator;
using ConstStr;

namespace Crawler;

public class AbstractBrowser : IBrowser
{
    protected IPData IPData { get; set; } = new();
    public TimeSpan Timeout
    {
        get => Driver.Manage().Timeouts().ImplicitWait;
        set => Driver.Manage().Timeouts().ImplicitWait = value;
    }
    private IWebElement _queryed;
    private IWebElement _queryedTmp;
    private IEnumerable<IWebElement> _queryedAll;
    protected KeyValuePair<ChromeOptions, ChromeDriverService> _chromeOptions;
    public IWebElement Queryed => _queryed;
    public IEnumerable<IWebElement> QueryedAll => _queryedAll;
    internal string RemoteUri { get; set; } = "";
    internal ChromeDriver Driver { get; set; } = null;
    protected AbstractBrowser()
    {
        _chromeOptions = new KeyValuePair<ChromeOptions, ChromeDriverService>(new ChromeOptions(), ChromeDriverService.CreateDefaultService());
        _chromeOptions.Value.HideCommandPromptWindow = true;
        _chromeOptions.Key.AddArgument("--incognito");
        _chromeOptions.Key.AddArgument("--private");
        _chromeOptions.Key.AddArgument("--lang=en");
    }
    public virtual async Task<IWebDriver> GetDriver()
    {
        return Driver;
    }
    public virtual async Task<IBrowser> ChangeIp(string country)
    {
        var uri = $"{Config.Instance.MissionHackerConfig.General!.ProxyApi}{Api.PROXY_API}".Replace("COUNTRY", country);
        var client = new HttpClient();
        var response = await client.GetStringAsync(uri);
        return this;
    }
    public virtual IPData GetIPData()
    {
        return this.IPData;
    }
    public IBrowser SendKeysByCss(string css, string keys)
    {
        var ele = Selector(css);
        ele.SendKeys(keys);
        return this;
    }
    public IBrowser ClickByCss(string css)
    {
        var ele = Selector(css);
        ele.Click();
        return this;
    }
    public IBrowser Click()
    {
        _queryed.Click();
        return this;
    }
    public IBrowser SendKeys(string keys)
    {
        _queryed.SendKeys(keys);
        return this;
    }
    public IBrowser QuerySelector(string css)
    {
        if (_queryedTmp is null)
        {
            _queryedTmp = Selector(css);
        }
        else
        {
            _queryedTmp = _queryedTmp.FindElement(By.CssSelector(css));
        }
        return this;
    }
    public IBrowser QuerySelectorAll(string css)
    {
        if (_queryedTmp is null)
        {
            _queryedAll = SelectMulti(css);
        }
        else
        {
            _queryedAll = _queryedTmp.FindElements(By.CssSelector(css));
        }
        return this;
    }
    public IBrowser SelectByCss(string css, int index)
    {
        var selector = new SelectElement(Driver.FindElement(By.CssSelector(css)));
        selector.SelectByIndex(index);
        return this;
    }
    public IBrowser SelectByCss(string css, string name)
    {
        var selector = new SelectElement(Driver.FindElement(By.CssSelector(css)));
        selector.SelectByValue(name);
        return this;
    }
    public IBrowser ClickEleOfIframe(string iframeCss, string eleCss)
    {
        Driver.SwitchTo().Frame(Driver.FindElement(By.CssSelector(iframeCss)));
        ClickByCss(eleCss);
        Driver.SwitchTo().DefaultContent();
        return this;
    }
    public IBrowser Clear()
    {
        _queryed.Clear();
        return this;
    }
    public async Task<IBrowser> WaitUntilNull(string css, int timeout)
    {
        var t = Timeout;
        for (var i = 0; i < timeout; i++)
        {
            try
            {
                Driver.Manage().Timeouts().ImplicitWait = TimeSpan.Zero;
                Driver.FindElement(By.CssSelector(css));
                await Task.Delay(1000);
            }
            catch
            {
                Driver.Manage().Timeouts().ImplicitWait = t;
                break;
            }
        }
        Driver.Manage().Timeouts().ImplicitWait = t;
        return this;
    }
    public IBrowser End()
    {
        if (_queryedTmp is null)
        {
            return this;
        }
        _queryed = _queryedTmp;
        _queryedTmp = null;
        return this;
    }
    public IBrowser ClickByJsCss(string css)
    {
        try
        {
            Driver.FindElement(By.CssSelector(css));
        }
        catch (Exception e)
        {

        }
        Driver.ExecuteJavaScript($"document.querySelector('{css}').click()");
        return this;
    }
    public async Task Quit()
    {
        Driver.Quit();
    }
    public virtual async Task<IBrowser> RefreshIpData()
    {
        var winHandler = Driver.CurrentWindowHandle;
        Driver.Manage().Cookies.DeleteAllCookies();
        Timeout = TimeSpan.FromSeconds(120);
        Driver.SwitchTo().NewWindow(WindowType.Tab);
        Driver.Url = "http://ip-api.com/json";
        var output = Driver.FindElement(By.CssSelector("body > pre")).Text;
        IPData = JsonSerializer.Deserialize<IPData>(output);
        Driver.Close();
        Driver.SwitchTo().Window(winHandler);
        return this;
    }
    // public IBrowser SetTimeout(TimeSpan span)
    // {
    //     Driver.Manage().Timeouts().ImplicitWait = span;
    //     return this;
    // }
    // public TimeSpan GetTimeout()
    // {
    //     return Driver.Manage().Timeouts().ImplicitWait;
    // }
    protected void InitChrome()
    {
        _chromeOptions.Key.DebuggerAddress = RemoteUri;
        this.Driver = new ChromeDriver(_chromeOptions.Value, _chromeOptions.Key);
        Timeout = TimeSpan.FromSeconds(120);
    }

    private IWebElement Selector(string css)
    {
        var ele = Driver.FindElement(By.CssSelector(css));
        return ele;
    }
    private IEnumerable<IWebElement> SelectMulti(string css)
    {
        var ele = Driver.FindElements(By.CssSelector(css));
        return ele;
    }
}