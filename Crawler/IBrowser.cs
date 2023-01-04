using OpenQA.Selenium;

namespace Crawler;

public interface IBrowser
{
    public Task<IWebDriver> GetDriver();
    public IBrowser ChangeIp(string country);
}