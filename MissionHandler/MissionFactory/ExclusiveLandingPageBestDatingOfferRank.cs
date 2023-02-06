using System.Security.Cryptography;
using Crawler;
using MissionHandler.MissionFactory.AbstractHandler;
using Models;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.Extensions;

namespace MissionHandler.MissionFactory;

public class ExclusiveLandingPageBestDatingOfferRank : AbstractMissionHandler
{
    public MailChrome MailChrome { get; set; }
    public IBrowser Browser { get; set; }
    public MissionInfo Info { get; set; }
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
    public override async Task<IMissionHandler> RunAsync()
    {
        await Task.Delay(500);
        MainDriver.SwitchTo().Window(MainDriver.WindowHandles.Last());
        await Task.Delay(500);
        var selection = MainDriver.FindElements(By.CssSelector(".single-portal"));
        var link = "";
        // var selectionNum = RandomNumberGenerator.GetInt32(0, 2);
        var selectionNum = 1;
        switch (selectionNum)
        {
            case 0:
                link = selection[0].FindElement(By.CssSelector(".main-btn")).GetAttribute("href");
                MainDriver.Navigate().GoToUrl(link);
                await SelectionOne();
                break;
            case 1: // 选择 asian Flirts
                link = selection[1].FindElement(By.CssSelector(".main-btn")).GetAttribute("href");
                MainDriver.Navigate().GoToUrl(link);
                await SelectionTwo();
                break;
        }
        return this;
    }
    public IMissionHandler SetInfo(MissionInfo info)
    {
        this.Info = info;
        return this;
    }
    private async Task<IMissionHandler> SelectionOne()
    {
        return this;
    }
    private async Task<IMissionHandler> SelectionTwo()
    {
        MainDriver.SwitchTo().Window(MainDriver.WindowHandles.Last());
        while (true)
        {
            try
            {
                await Task.Delay(1000);
                var loading = MainDriver.FindElement(By.CssSelector("body > div > div > div:nth-child(2) > p:nth-child(1)"));
                if (loading is null)
                {
                    break;
                }
            }
            catch
            {
                break;
            }
        }
        var selectionOfAsianFlirts = 0;
        try
        {
            var signUpBtn = MainDriver.FindElement(By.Id("signUp"));
            if (signUpBtn is null)
            {
                selectionOfAsianFlirts = 1;
            }
        }
        catch
        {
            selectionOfAsianFlirts = 1;
        }
        switch (selectionOfAsianFlirts)
        {
            case 0: // 一上来就要填邮箱的界面
                Console.WriteLine("asianflirts 1"); 
                return await AsianFlirtsPageOne();
            case 1: // 左下角有个按钮的界面
                Console.WriteLine("asianflirts 2");
                return await AsianFlirtsPageTwo();
        }
        return this;
    }
    private async Task<IMissionHandler> AsianFlirtsPageOne()
    {
        MainDriver.FindElement(By.CssSelector("#signupForm > input[type=email]:nth-child(22)")).SendKeys(Info.Mail.MailName);
        await Awaiter.GetDelay();
        MainDriver.FindElement(By.Id("signUp")).Click();
        return this;
    }
    private async Task<IMissionHandler> AsianFlirtsPageTwo()
    {
        MainDriver.FindElement(By.CssSelector("body > div.main > div > div > button"));
        return this;
    }
}