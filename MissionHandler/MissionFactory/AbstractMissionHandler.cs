using Crawler;
using Models;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.Extensions;

namespace MissionHandler.MissionFactory;

public class AbstractMissionHandler : IMissionHandler
{
    public MailChrome MailChrome { get; set; }
    public IBrowser Browser { get; set; }
    public MissionInfo info { get; set; }
    internal IWebDriver MainDriver { get; set; }
    internal IWebDriver MailDriver { get; set; }
    public virtual IMissionHandler Init()
    {
        MainDriver.Navigate().GoToUrl(info.Links.Domain);
        MainDriver.SwitchTo().Window(MainDriver.WindowHandles.Last());
        MainDriver.ExecuteJavaScript($"window.open('{info.Links.Link}')");
        MainDriver.SwitchTo().Window(MainDriver.WindowHandles.Last());
        return this;
    }
    public async Task<IMissionHandler> SetMailBrowser(MailChrome browser)
    {
        MailChrome = browser;
        MailDriver = await browser.GetDriver(info.Mail);
        return this;
    }
    public async Task<IMissionHandler> SetBrowser(IBrowser browser)
    {
        Browser = browser;
        MainDriver = await browser.GetDriver();
        return this;
    }
    public IMissionHandler SetInfo(MissionInfo info)
    {
        this.info = info;
        return this;
    }
    public async Task<IMissionHandler> Wait()
    {
        await Awaiter.GetDelay();
        return this;
    }
    public async Task<IMissionHandler> Wait(int ms)
    {
        await Task.Delay(ms);
        return this;
    }
    public async virtual Task<IMissionHandler> RunAsync()
    {
        return this;
    }
}