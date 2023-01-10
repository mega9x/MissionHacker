using Models.Data;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;

namespace Crawler;

public class AbstractBrowser : IBrowser
{
    public IPData IPData { get; set; } = new();
    public TimeSpan Timeout
    {
        get => Driver.Manage().Timeouts().ImplicitWait; 
        set => Driver.Manage().Timeouts().ImplicitWait = value; 
    }
    private IWebElement _queryed;
    private IWebElement _queryedTmp;
    private IEnumerable<IWebElement> _queryedAll;
    public IWebElement Queryed => _queryed;
    public IEnumerable<IWebElement> QueryedAll => _queryedAll;

    internal ChromeDriver Driver { get; set; } = null;
    public virtual Task<IWebDriver> GetDriver()
    {
        throw new NotImplementedException();
    }
    public virtual IBrowser ChangeIp(string country)
    {
        throw new NotImplementedException();
    }
    public virtual IPData GetIPData()
    {
        throw new NotImplementedException();
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
    public IBrowser Clear()
    {
        _queryed.Clear();
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
    // public IBrowser SetTimeout(TimeSpan span)
    // {
    //     Driver.Manage().Timeouts().ImplicitWait = span;
    //     return this;
    // }
    // public TimeSpan GetTimeout()
    // {
    //     return Driver.Manage().Timeouts().ImplicitWait;
    // }
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