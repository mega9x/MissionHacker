using Models.Data;
using OpenQA.Selenium;

namespace Crawler;

public interface IBrowser
{
    public Task<IWebDriver> GetDriver();
    public IBrowser ChangeIp(string country);
    public IPData GetIPData();
    public IBrowser SendKeysByCss(string css, string keys);
    public IBrowser ClickByCss(string css);
    public IWebElement QuerySelector(string css);
}