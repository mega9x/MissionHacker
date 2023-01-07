using Crawler;
using Models;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using UserAgentGenerator;
using OpenQA.Selenium.Chrome;
using System.Security.Cryptography;
using System.Reflection.Metadata;
using InfoGen;
using MissionHandler.Utils;
using OpenQA.Selenium.Support.Extensions;
using System.Xml.Linq;

namespace MissionHandler.MissionFactory;

public class FlirtDating : AbstractMissionHandler
{
    public MailChrome MailChrome { get; set; }
    public IBrowser Browser { get; set; }
    public MissionInfo info { get; set; }
    private IWebDriver MainDriver { get; set; }
    private IWebDriver MailDriver { get; set; }
    public IMissionHandler SetMailBrowser(MailChrome browser)
    {
        MailChrome = browser;
        return this;
    }
    public IMissionHandler SetBrowser(IBrowser browser)
    {
        Browser = browser;
        return this;
    }
    public IMissionHandler SetInfo(MissionInfo info)
    {
        this.info = info;
        return this;
    }
    public async Task<IMissionHandler> RunAsync()
    {
        if(MailChrome is null && Browser is null)
        {
            throw new NullReferenceException();
        }
        MainDriver = await Browser.GetDriver();
        MailDriver = await MailChrome.GetDriver(info.Mail);
        MainDriver.Navigate().GoToUrl(info.Links.Domain);
        MainDriver.SwitchTo().Window(MainDriver.WindowHandles.Last());
        MainDriver.ExecuteJavaScript($"window.open('{info.Links.Link}')");
        MainDriver.SwitchTo().Window(MainDriver.WindowHandles.Last());
        await Task.Delay(8000);
        var allInput = MainDriver.FindElements(By.CssSelector(".form-input > input"));
        var allSelect = MainDriver.FindElements(By.CssSelector(".form-select"));
        allSelect[0].Click();
        await Task.Delay(1000);
        // 选择性取向
        allSelect[0].FindElements(By.CssSelector(".select-item"))[RandomNumberGenerator.GetInt32(0, 4)].Click();
        await Awaiter.GetDelay();
        MainDriver.FindElement(By.CssSelector(".next-btn")).Click();
        await Task.Delay(1000);
        allSelect[2].Click();
        await Task.Delay(1000);
        // 选择岁数
        allSelect[2].FindElements(By.CssSelector(".select-item"))[RandomNumberGenerator.GetInt32(17, 30)].Click();
        await Awaiter.GetDelay();
        MainDriver.FindElement(By.CssSelector(".next-btn")).Click();
        await Task.Delay(800);
        // 填写 Zip Code
        allInput[0].SendKeys("95993");
        await Awaiter.GetDelay();
        MainDriver.FindElement(By.CssSelector(".next-btn")).Click();
        await Task.Delay(800);
        // 填写邮箱
        allInput[1].SendKeys(info.Mail.MailName);
        await Awaiter.GetDelay();
        MainDriver.FindElement(By.CssSelector(".next-btn")).Click();
        await Task.Delay(800);
        // 填写密码
        allInput[2].SendKeys(info.Mail.MailPd);
        await Awaiter.GetDelay();
        MainDriver.FindElement(By.CssSelector(".submit-btn")).Click();
        MailChrome.ClickLatestOther("Flirt");
        await Task.Delay(800);
        var href = MailDriver.FindElement(By.CssSelector("#ReadingPaneContainerId > div > div > div > div.L72vd > div > div:nth-child(2) > div.aVla3 > div > div > div > div > div.XbIp4.jmmB7.GNqVo.yxtKT.allowTextSelection > div > div > div > table > tbody > tr:nth-child(3) > td > center > table > tbody > tr:nth-child(4) > td > table > tbody > tr:nth-child(4) > td:nth-child(2) > a"))
            .GetAttribute("href");
        MainDriver.Navigate().GoToUrl(href);
        return this;
    }
    private async Task FirstPage()
    {
        var driver = await Browser.GetDriver();
        driver.Navigate().GoToUrl(info.Links.Domain);
        driver.ExecuteJavaScript($"window.open('{info.Links.Link}')");
        driver.SwitchTo().Window(driver.WindowHandles.Last());
        var allInput = driver.FindElements(By.CssSelector(".form-input > input"));
        var allSelect = driver.FindElements(By.CssSelector("select"));
        new SelectElement(allSelect[0]).SelectByIndex(RandomNumberGenerator.GetInt32(0, 4));
        await Task.Delay(500);
        driver.FindElement(By.CssSelector(".next-btn")).Click();
        new SelectElement(allSelect[2]).SelectByIndex(RandomNumberGenerator.GetInt32(0, 30));
        await Awaiter.GetDelay();
        allInput[0].SendKeys(Browser.GetIPData().CurrentZip);
        driver.FindElement(By.CssSelector(".next-btn")).Click();
        await Awaiter.GetDelay();
        allInput[1].SendKeys(info.Mail.MailName);
        await Awaiter.GetDelay();
        allInput[2].SendKeys(StrGen.GetPassword());
    }
}