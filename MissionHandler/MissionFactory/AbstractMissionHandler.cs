using System.Security.Cryptography;
using Crawler;
using Models;
using Models.Data;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.Extensions;
using OutlookHacker.Main.MailName;
using Utils.PersonFactory;

namespace MissionHandler.MissionFactory;

public class AbstractMissionHandler : IMissionHandler
{
    internal IWebDriver MainDriver { get; set; }
    internal IWebDriver MailDriver { get; set; }
    public SinglePerson Person { get; private set; } = global::Utils.PersonFactory.Person.GetRandomPerson();
    public IPData IpData { get; private set; }
    public MailChrome MailChrome { get; set; }
    public IBrowser Browser { get; set; }
    public MissionInfo info { get; set; }
    public virtual IMissionHandler Init()
    {
        if (!info.Links.Domain.StartsWith("http"))
        {
            info.Links.Domain = $"https://{info.Links.Domain}";
        }
        MainDriver.Url = info.Links.Domain;
        MainDriver.SwitchTo().Window(MainDriver.WindowHandles.Last());
        MainDriver.ExecuteJavaScript($"window.open('{info.Links.Link}')");
        MainDriver.SwitchTo().Window(MainDriver.WindowHandles.Last());
        return this;
    }
    public async Task<IMissionHandler> SetMailBrowser(MailChrome browser)
    {
        MailChrome = browser;
        MailDriver = await browser.GetDriver();
        return this;
    }
    public async Task<IMissionHandler> SetBrowser(IBrowser browser)
    {
        Browser = browser;
        MainDriver = await browser.GetDriver();
        IpData = browser.GetIPData();
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