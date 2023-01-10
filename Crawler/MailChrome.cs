using Events;
using Events.EventArgs;
using MissionHacker.ConfigHelper;
using Models;
using Models.Data;
using System.Text.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.Extensions;

namespace Crawler;

public class MailChrome : AbstractBrowser
{
    public bool Locked { get; private set; } = false;
    public IWebElement InboxBtn { get; set; }
    public IWebElement TrashBtn { get; set; }
    public IWebElement FocusedBtn { get; set; }
    public IWebElement OtherBtn { get; set; }
    public async Task<IWebDriver> GetDriver(Mail mail)
    {
        if (Driver is null)
        {
            var chromeOption = new ChromeOptions();
            chromeOption.AddArgument("--incognito");
            chromeOption.AddArgument("--private");
            chromeOption.AddArgument("--lang=en");
            var defaultService = ChromeDriverService.CreateDefaultService(".\\driver");
            defaultService.HideCommandPromptWindow = true;
            Driver = new(defaultService, chromeOption);
            Driver.Manage().Window.Size = new(800, 720);
            Timeout = TimeSpan.FromSeconds(60);
            Driver.Url = "http://ip-api.com/json";
            var output = Driver.FindElement(By.CssSelector("body > pre")).Text;
            IPData = JsonSerializer.Deserialize<IPData>(output);
            Driver.Manage().Cookies.DeleteAllCookies();
            Driver.Url = "https://outlook.live.com/owa/?cobrandid=ab0455a0-8d03-46b9-b18b-df2f57b9e44c&nlp=1&deeplink=owa/?realm=outlook.com"; 
            Driver.FindElement(By.Id("i0116")).SendKeys(mail.MailName);
            await Task.Delay(500);
            Driver.FindElement(By.Id("idSIButton9")).Click();
            Driver.FindElement(By.Id("i0118")).SendKeys(mail.MailPd);
            await Task.Delay(3000);
            Driver.FindElement(By.Id("idSIButton9")).Click();
            try
            {
                await Task.Delay(500);
                Driver.FindElement(By.Id("idBtn_Back")).Click();
            }
            catch
            {
                // ignore
            }
        }
        return this.Driver;

    }
    public async Task<MailChrome> ClickFocused()
    {
        await FindLeftBtns();
        InboxBtn.Click();
        var msPivot = Driver.FindElements(By.CssSelector(".ms-Pivot-linkContent"));
        FocusedBtn = msPivot[0];
        OtherBtn = msPivot[1];
        FocusedBtn.Click();
        return this;
    }

    public async Task<MailChrome> ClickOther()
    {
        await FindLeftBtns();
        InboxBtn.Click();
        var msPivot = Driver.FindElements(By.CssSelector(".ms-Pivot-linkContent"));
        FocusedBtn = msPivot[0];
        OtherBtn = msPivot[1];
        OtherBtn.Click();
        return this;
    }
    public async Task<MailChrome> ClickTrash()
    {
        await FindLeftBtns();
        TrashBtn.Click();
        return this;
    }
    public async Task<MailChrome> ClickMailFromCurrent(string keyword)
    {
        if (Locked || Driver is null)
        {
            return this;
        }
        var lower = keyword.ToLower();
        Locked = true;
        var timeout = 0;
        List<IWebElement> selected = new();
        while (timeout <= 60)
        {
            await Task.Delay(1000);
            timeout++;
            try
            {
                selected = await FindMails(lower);
                if (selected is null || selected.Count <= 0)
                {
                    continue;
                }
                selected[0].Click();
            }
            catch(Exception e)
            {

            }
            break;
        }
        Locked = false;
        return this;
    }
    public async Task<MailChrome> ClickMail(string keyword)
    {
        List<Func<Task>> allAction = new()
        {
            async () => await ClickFocused(),
            async () => await ClickOther(),
            async () => await ClickTrash()
        };
        if (Locked || Driver is null)
        {
            return this;
        }
        var lower = keyword.ToLower();
        Locked = true;
        var timeout = 0;
        List<IWebElement> selected = new();
        while (timeout <= 60)
        {
            bool isSelected = false;
            foreach (var a in allAction)
            {
                await a();
                await Task.Delay(1000);
                await ClickFocused();
                timeout++;
                try
                {
                    selected = await FindMails(lower);
                    if (selected is null || selected.Count <= 0)
                    {
                        continue;
                    }
                    selected[0].Click();
                    isSelected = true;
                }
                catch(Exception e)
                {
                    
                }
                break;
            }
            if (isSelected) break;
        }
        Locked = false;
        return this;
    }
    public async Task<List<IWebElement>> FindMails(string keyword)
    {
        var selected = Driver.FindElements(By.CssSelector(".hcptT"));
        return selected.Where(x => x.GetAttribute("aria-label").ToLower().Contains(keyword)).ToList();
    }
    public async Task FindLeftBtns()
    {
        while (true)
        {
            try
            {
                var leftBtns = Driver.FindElements(By.CssSelector(".C2IG3"));
                InboxBtn = leftBtns[3];
                TrashBtn = leftBtns[4];
                await Task.Delay(1000);
                break;
            }
            catch (Exception e)
            {
                Driver.ExecuteJavaScript("document.querySelector('#innerRibbonContainer button').click()");
            }
        }
    }
}