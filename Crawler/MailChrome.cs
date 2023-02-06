using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography;
using System.Text;
using Events;
using Events.EventArgs;
using MissionHacker.ConfigHelper;
using Models;
using Models.Data;
using System.Text.Json;
using System.Web;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.Extensions;
using Utils.Text;
using WebSocketSharp;
using Zu.AsyncWebDriver.Internal;

namespace Crawler;

public class MailChrome : AbstractBrowser
{
    public bool Locked { get; private set; } = false;
    public IWebElement InboxBtn { get; set; }
    public IWebElement TrashBtn { get; set; }
    public IWebElement FocusedBtn { get; set; }
    public IWebElement OtherBtn { get; set; }
    public Mail Mail { get; set; }
    public MailChrome(): base()
    {
        if (Config.Config.Instance.MissionHackerConfig.General.Proxy.Length > 0)
        {
            _chromeOptions.Key.AddArgument($"--proxy-server=socks5://{Config.Config.Instance.MissionHackerConfig.General.Proxy}");
        }
    }
    public MailChrome(Mail mail): this()
    {
        Mail = mail;
    } 
    public override async Task<IWebDriver> GetDriver()
    {
        if (Driver is not null) return this.Driver;
        await Login();
        return this.Driver;
    }
    public async Task<MailChrome> Login()
    {
        InitChrome();
        Driver.Manage().Cookies.DeleteAllCookies();
        Timeout = TimeSpan.FromSeconds(300);
        Driver.Manage().Window.Size = new(1200, 720);
        Driver.Url = "https://outlook.live.com/owa/?cobrandid=ab0455a0-8d03-46b9-b18b-df2f57b9e44c&nlp=1&deeplink=owa/?realm=outlook.com";
        Driver.FindElement(By.Id("i0116")).SendKeys(Mail.MailName);
        await Task.Delay(500);
        Driver.FindElement(By.Id("idSIButton9")).Click();
        Driver.FindElement(By.Id("i0118")).SendKeys(Mail.MailPd);
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
        await Task.Delay(1000);
        return this;
    }
    public async Task<MailChrome> Quit()
    {
        Driver.Quit();
        Driver = null;
        return this;
    }
    public async Task<MailChrome> ClickInbox()
    {
        var doNotFocused =
            "Y29uc3Qgc2xlZXAgPSBhc3luYyAoKSA9PiB7CiAgICByZXR1cm4gbmV3IFByb21pc2UoeCA9PiBzZXRUaW1lb3V0KHgsIDIwMDApKTsKfQoKbGV0IGZlYXR1cmVzID0gZG9jdW1lbnQucXVlcnlTZWxlY3RvcigiW2FyaWEtbGFiZWw9J0FjY2VzcyBhZGRpdGlvbmFsIGZlYXR1cmVzJ10iKTsKaWYoZmVhdHVyZXMgIT0gbnVsbCkgewogICAgZmVhdHVyZXMuY2xpY2soKTsKfQpsZXQgZm9jdXNlZDsKd2hpbGUodHJ1ZSkgewogICAgdHJ5IHsKICAgICAgICBkb2N1bWVudC5xdWVyeVNlbGVjdG9yKCIjb3dhU2V0dGluZ3NCdXR0b24iKS5jbGljaygpOwogICAgfSBjYXRjaCB7fQogICAgZm9jdXNlZCA9IGRvY3VtZW50LnF1ZXJ5U2VsZWN0b3IoIlthcmlhLWxhYmVsPSdGb2N1c2VkIEluYm94J10iKTsKICAgIGlmKGZvY3VzZWQgPT09IG51bGwpIHsKICAgICAgICBhd2FpdCBzbGVlcCgpOwogICAgICAgIGNvbnRpbnVlOwogICAgfQogICAgYnJlYWs7Cn0KaWYoZm9jdXNlZC5nZXRBdHRyaWJ1dGUoImFyaWEtY2hlY2tlZCIpLmluY2x1ZGVzKCJ0cnVlIikpIHsKICAgIGZvY3VzZWQuY2xpY2soKTsKfQpkb2N1bWVudC5xdWVyeVNlbGVjdG9yKCIjZmxleFBhbmVDbG9zZUJ1dHRvbiIpLmNsaWNrKCk7".DecodeBase64();
        try
        {
            Driver.Manage().Timeouts().AsynchronousJavaScript = TimeSpan.FromSeconds(120);
            Driver.ExecuteJavaScript(doNotFocused);
        }
        catch (Exception e)
        {
            MissionEvents.ThrowException(this, e, "设置邮箱时出了问题: 设置是否将收件箱分为重点和其他");
        }
        await FindLeftBtns();
        InboxBtn.Click();
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
            catch
            {
                // ignore
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
            async () => await ClickInbox(),
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
        return selected.Where(x => x.GetAttribute("outerHTML").ToLower().Contains(keyword)).ToList();
    }
    public async Task FindLeftBtns()
    {
        while (true)
        {
            try
            {
                InboxBtn = Driver.FindElement(By.CssSelector("[title='Inbox']"));
                TrashBtn = Driver.FindElement(By.CssSelector("[title='Junk Email']"));
                await Task.Delay(1000);
                break;
            }
            catch (Exception e)
            {
                Driver.ExecuteJavaScript("document.querySelector(\"[aria-label='Toggle left pane']\").click()");
            }
        }
    }
    public async Task<IWebElement> GetMailRoot()
    {
        return Driver.FindElement(By.CssSelector("[aria-label='Message body'] tbody"));
    }
    public async Task<string> GetHrefByKeyword(string keyword)
    {
        var time = 0;
        bool Timeout() => time >= 60;
        while (!Timeout())
        {
            time++;
            try
            {
                await Task.Delay(1000);
                var mail = await GetMailRoot();
                var e = mail.FindElements(By.TagName("a")).Where(x => HttpUtility.HtmlDecode(x.GetAttribute("outerHTML")).ToLower().Contains(keyword.ToLower())).ToArray();
                return e[0].GetAttribute("href");
            }
            catch (Exception e)
            {
                if (Timeout())
                {
                    throw;
                }
            }
        }
        return null;
    }
    public async Task<string> GetHrefByIndex(int index)
    {
        var mail = await GetMailRoot();
        var e = mail.FindElements(By.TagName("a"));
        return e[index].GetAttribute("href");
    }
}