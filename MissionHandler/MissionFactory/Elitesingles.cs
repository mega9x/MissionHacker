using System.Collections.ObjectModel;
using System.Net.Mime;
using System.Security.Cryptography;
using ChanceNET;
using Crawler;
using Events;
using Events.EventArgs;
using MissionHandler.InfoGen;
using Models;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.Extensions;
using OutlookHacker.Main.MailName;
using UserAgentGenerator;

namespace MissionHandler.MissionFactory;

public class Elitesingles : AbstractMissionHandler
{
    private int sex = RandomNumberGenerator.GetInt32(0, 2);
    private string genderSelectorOne = "";
    private string genderSelectorOne_LookingFor = "";
    private string genderSelectorTwo = "";
    private string genderSelectorTwo_LookingFor = "";
    private int tall = 0;
    private bool tallAnswerd = false;
    private const string mailConfirmXPath = "/html/body/div[2]/div/div[2]/div[2]/div[2]/div[1]/div/div/div[3]/div/div[3]/div[3]/div/div/div/div/div[2]/div/div[2]/div[1]/div/div/div/div/div[3]/div/div/div/div/table/tbody/tr/td/table[2]/tbody/tr/td/table[3]/tbody/tr/td[2]/table/tbody/tr[3]/td[2]/table/tbody/tr/td/div/a";
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
    public override async Task<IMissionHandler> RunAsync()
    {
        MailChrome.Mail = info.Mail;
        await MailChrome.Login();
        tall = sex == 0 ? RandomNumberGenerator.GetInt32(169, 190) : RandomNumberGenerator.GetInt32(150, 180);
        var random = new RandomGen();
        var nickname = random.GetRandomNameWithoutEnding();
        if (sex == 0)
        {
            genderSelectorOne = "#genderMale";
            genderSelectorOne_LookingFor = "#searchedGenderFemale";
            genderSelectorTwo =
                "#psytest > div > div > main > div > div.section.single > div > div > div > button:nth-child(2)";
            genderSelectorTwo_LookingFor =
                "#psytest > div > div > main > div > div.section.single > div > div > div > button:nth-child(1)";
        }
        else
        {
            genderSelectorOne = "#genderFemale";
            genderSelectorOne_LookingFor = "#searchedGenderMale";
            genderSelectorTwo =
                "#psytest > div > div > main > div > div.section.single > div > div > div > button:nth-child(1)";

            genderSelectorTwo_LookingFor =
                "#psytest > div > div > main > div > div.section.single > div > div > div > button:nth-child(2)";
        }
        var birthyear = RandomNumberGenerator.GetInt32(1957, 1994);
        var age = DateTime.Today.Year - birthyear;
        var ageSelectorIndex = age switch
        {
            > 18 and < 21 => 0,
            > 22 and < 27 => 1,
            > 28 and < 39 => 2,
            > 40 and < 49 => 3,
            > 50 and < 59 => 4,
            > 60 and < 69 => 5,
            _ => 0
        };
        MainDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(180);
        // 打开年龄选择器
        var password = random.GetRandomName() + RandomNumberGenerator.GetInt32(0, 9999);
        MainDriver.ExecuteJavaScript($"document.querySelector('{genderSelectorOne}').click()");
        MainDriver.ExecuteJavaScript($"document.querySelector('{genderSelectorOne_LookingFor}').click()");
        MainDriver.FindElement(By.CssSelector("#custom-select")).Click();
        MainDriver.FindElements(By.CssSelector(".ageRange-list > div"))[ageSelectorIndex].Click();
        MainDriver.FindElement(By.CssSelector("#emailaddy")).SendKeys(info.Mail.MailName);
        MainDriver.FindElement(By.CssSelector("#legal")).Click();
        Browser.ClickByJsCss("#submit-btn-01");
        await Wait();
        MainDriver.FindElement(By.CssSelector("#password")).SendKeys(password);
        await Wait(1000);
        MainDriver.FindElement(By.CssSelector("#passwordRepeat")).SendKeys(password);
        Browser.ClickByCss("#submit-btn");
        MainDriver.SwitchTo().Frame(Browser.QuerySelector("#h-captcha-wrapper > iframe").End().Queryed);
        Browser.ClickByCss("#anchor-state");
        MainDriver.SwitchTo().ParentFrame();
        await Wait(1000);
        await Wait();
        Browser.ClickByCss(
            "#psytest > div > div > main > div > div.section.single.candy-screen > div > footer > button.btn.btn-primary.continue");
        await Wait();
        await Wait(1200);
        Browser.ClickByJsCss(genderSelectorTwo);
        await Wait();
        await Wait(1200);
        Browser.ClickByJsCss(genderSelectorTwo_LookingFor);
        await Wait();
        await Wait(1200);
        while (true)
        {
            await Wait();
            await Wait(1200);
            var result = await AutoSelect();
            if (!result)
            {
                break;
            }
        }
        // Please enter your first name
        await Wait();
        await Wait(1200);
        Browser.SendKeysByCss(
            "#psytest > div > div > main > div > div.section.single > div > div > div > div > div > div > div > label > input",
            nickname);
        await Wait();
        await Wait(1200);
        ClickBigGreenNextBtn();
        await Wait();
        await Wait(1200);
        Browser.SendKeysByCss(
            "#psytest > div > div > main > div > div.section.single > div > div > div.location-answer > div.spark-form-group > div > div > div > label > input",
            Browser.GetIPData().CurrentZip);
        await Wait();
        await Wait(1200);
        Browser.ClickByCss(
            "body > div.spark-autocomplete-suggestions.spark-autocomplete-bottom > div.spark-autocomplete-suggestion.to-select");
        ClickBigGreenNextBtn();
        await Wait();
        await Wait(1200);
        await AutoSelect();
        await Wait();
        await Wait(1200);
        await AutoSelect();
        var date = Date.GenRandomDate();
        await Wait();
        await Wait(1200);
        Browser.ClickByCss("#psytest > div > div > main > div > div.section.single > div > div > div > div.d-flex > div.date-input-field.month-field > div > div > div.spark__value-container.css-1hwfws3");
        await Wait(1200);
        MainDriver.FindElements(By.CssSelector(".spark__option"))[date.Month - 1].Click();
        await Wait(1200);
        Browser.ClickByCss(
            "#psytest > div > div > main > div > div.section.single > div > div > div > div.d-flex > div.date-input-field.day-field > div > div > div.spark__value-container.css-1hwfws3");
        MainDriver.FindElements(By.CssSelector(".spark__option"))[date.Day - 1].Click();
        await Wait(1200);
        Browser.ClickByCss("#year-select-field > div > div.year-select.spark__value-container.css-1hwfws3");
        await Wait(1200);
        ReadOnlyCollection<IWebElement>? dropDown = MainDriver.FindElements(By.CssSelector(".spark__option"));
        dropDown.First(x => x.Text == birthyear.ToString()).Click();
        await Wait();
        await Wait(1200);
        ClickBigGreenNextBtn();
        await Wait();
        await Wait(1200);
        await AutoSelect();
        await Wait();
        await Wait(1200);
        Browser.SendKeysByCss(
            "#react-select-5-input",
            random.GetRandomProfession());
        ClickBigGreenNextBtn();
        while (true)
        {
            await Wait();
            await Wait(1200);
            var result = await AutoSelect();
            if (!result)
            {
                break;
            }
        }
        await Wait();
        await Wait(1200);
        Browser.ClickByCss(
            "#client > div.spark-page-with-nav-bar.onboarding-step.basic-user-view.hide-become-premium-banner > div > main > div > div > div.photowall_right > section > div > div.gallery-upload-zone-content > button.btn.btn-dark.btn-link.btn-block.spark-btn.btn-justify-content-center.btn-lg");
        await Wait();
        await Wait(1200);
        Browser.ClickByCss(
            "body > div.spark-modal > div.spark-modal-dialog.modal-lg > div.spark-modal-dialog-wrapper > div.spark-modal-dialog-body > button.btn.btn-dark.btn-link.btn-block.spark-btn.btn-justify-content-center");
        Browser.ClickByCss(
            "#client > div.spark-page-with-nav-bar.onboarding-step.basic-user-view.hide-become-premium-banner > div > main > div > div > div.profilefill_right > div > div.carousel__slides-container > section > div:nth-child(1) > div > button.q-carousel__button-answer.btn.btn-light.spark-btn.btn-justify-content-center");
        await Wait();
        await Wait(1200);
        Browser.ClickByCss(
            "body > div:nth-child(2) > main > div > div > section.subheader > div > div.back.hide-for-small > a");
        MainDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(20);
        await MailChrome.ClickMail("EliteSingles Confirm your email address to complete");
        try
        {

            var herf = await MailChrome.GetHrefByKeyword("Confirm my email address >>");
            MainDriver.SwitchTo().NewWindow(WindowType.Tab);
            MainDriver.Navigate().GoToUrl(herf);
        }
        catch(Exception e)
        {
            MissionEvents.ThrowException(this, e, "无法进行邮箱验证");
        }

        // MailChrome.ClickLatestFocused()
        return this;
    }
    private async Task<bool> AutoSelect()
    {
        await Browser.WaitUntilNull("#psytest > div > div > main > div.saving-spinner > div > svg", 300);
        var clicked = true;
        var timespan = Browser.Timeout;
        while(clicked)
        {
            try
            {
                Browser.Timeout = TimeSpan.FromSeconds(2);
                if (!tallAnswerd)
                {
                    // tall selector
                    Browser.QuerySelector(
                            "#psytest > div > div > main > div > div.section.single > div > div > div > div.spark-form-group.mb-0 > div > div.spark-form-group.body-height-form-group > div > div > div > label > input").End()
                        .SendKeys(Keys.Control + "a")
                        .SendKeys(Keys.Backspace)
                        .Clear()
                        .SendKeys(tall.ToString());
                    tallAnswerd = true;
                    break;
                }
                ClickBigGreenNextBtn();
                break;
            }
            catch(Exception e)
            {
                MissionEvents.ThrowException(this, new ThrowEventArgs()
                {
                    Exception = e,
                    FullMessage = e.ToString(),
                    SimpleMessage = "未能找到设置身高的相关元素",
                });
            }
            try
            {
                ClickBigGreenNextBtn();
                break;
            }
            catch (Exception e)
            {
                MissionEvents.ThrowException(this, new ThrowEventArgs()
                {
                    Exception = e,
                    FullMessage = e.ToString(),
                    SimpleMessage = "未能找到大大个的绿色下一步按钮",
                });
            }
            try
            {
                Browser.Timeout = TimeSpan.FromSeconds(5);
                var typeSelector = "#psytest > div > div > main > div > div.section.single > div > div > div";
                var className = Browser.QuerySelector(typeSelector).End().Queryed.GetAttribute("class");
                Console.WriteLine();
                if (className.Contains("scale-answer"))
                {
                    SelectAnswers();
                    break;
                }
                if (className.Contains("picture-answer"))
                {
                    SelectPic();
                    break;
                }
                if (className.Contains("choice-answer"))
                {
                    await SelectItem();
                    break;
                }
            }
            catch(Exception e)
            {
                MissionEvents.ThrowException(this, new ThrowEventArgs
                {
                    Exception = e,
                    FullMessage = e.ToString(),
                    SimpleMessage = "未能找到三种问答题的相关元素",
                });
            }
            try
            {
                ClickBigGreenContinueBtn();
                break;
            }
            catch(Exception e)
            {
                MissionEvents.ThrowException(this, new ThrowEventArgs
                {
                    Exception = e,
                    FullMessage = e.ToString(),
                    SimpleMessage = "未能找到大绿色 Continue 按钮",
                });
            }
            try
            {
                ClickIWillAnswerItLater();
                break;
            }
            catch(Exception e)
            {
                MissionEvents.ThrowException(this, new ThrowEventArgs
                {
                    Exception = e,
                    FullMessage = e.ToString(),
                    SimpleMessage = "未能找到 Answer it later",
                });
            }
            clicked = false;
        }
        MainDriver.Manage().Timeouts().ImplicitWait = timespan;
        return clicked;
    }
    private IWebElement SelectAnswers()
    {
        var selectorStr = "#psytest > div > div > main > div > div.section.single > div > div > div > div input";
        var selectors =
            MainDriver.FindElements(
                By.CssSelector(selectorStr));
        var i = RandomNumberGenerator.GetInt32(0, selectors.Count);
        var selector = selectors[i];
        MainDriver.ExecuteJavaScript(
            $"document.querySelectorAll('{selectorStr}')[{i}].click()");
        return selector;
    }
    private IWebElement SelectPic()
    {
        var selectorStr = "#psytest > div > div > main > div > div.section.single > div > div > div > div > label";
        var selectors =
            MainDriver.FindElements(
                By.CssSelector(
                    selectorStr));
        var index = RandomNumberGenerator.GetInt32(0, selectors.Count);
        selectors[index].Click();
        return selectors[index];
    }
    private async Task<IList<IWebElement>> SelectItem()
    {
        var selectorStr = "#psytest > div > div > main > div > div.section.single > div > div > div > div > div";
        var selectors = MainDriver.FindElements(By.CssSelector(selectorStr));
        var minTimes = 1;

        try
        {
            var choice = Browser.QuerySelector(".choice-hint").End().Queryed.Text;
            if (choice.Contains("4"))
            {
                minTimes = 5;
            }
        }
        catch(Exception e)
        {
            MissionEvents.ThrowException(this, new ThrowEventArgs
            {
                Exception = e,
                FullMessage = e.ToString(),
                SimpleMessage = "未能找到声明次数的元素",
            });
        }
        var times = RandomNumberGenerator.GetInt32(minTimes, selectors.Count);
        List<IWebElement> list = new();
        try
        {
            var timeout = Browser.Timeout;
            Browser.Timeout = TimeSpan.FromSeconds(3);
            var selectedIndex = RandomNumberGenerator.GetInt32(0, 3);
            var selected = MainDriver.FindElement(By.CssSelector("#psytest > div > div > main > div > div.section.single > div > div > header > h4 > span"));
            if (!selected.Text.ToLower().Contains("how did you hear about us") || RandomNumberGenerator.GetInt32(0, 101) < 80) throw new Exception();
            var google = selectors.First(x => x.Text.ToLower().Contains("google"));
            var facebook = selectors.First(x => x.Text.ToLower().Contains("facebook"));
            var youtube = selectors.First(x => x.Text.ToLower().Contains("youtube"));
            Browser.Timeout = timeout;
            switch (selectedIndex)
            {
                case 1:
                    list.Add(facebook);
                    facebook.Click();
                    return list;
                case 0:
                    list.Add(google);
                    google.Click();
                    return list;
                case 2:
                    list.Add(youtube);
                    youtube.Click();
                    return list;
            }
        }
        catch (Exception e)
        {
        }
        var before = 0;
        for (int i = 0; i < times; i++)
        {
            var clickIndex = RandomNumberGenerator.GetInt32(0, selectors.Count);
            while (clickIndex == before)
            {
                clickIndex = RandomNumberGenerator.GetInt32(0, selectors.Count);
                await Wait(100);
            }
            before = clickIndex;
            var selector = selectors[clickIndex];
            MainDriver.ExecuteJavaScript(
                $"document.querySelectorAll(\"#psytest > div > div > main > div > div.section.single > div > div > div > div > div\")[{clickIndex}].click()");
            list.Add(selector);
            await Wait(100);
        }
        return list;
    }
    private void ClickBigGreenNextBtn()
    {
        Browser.ClickByCss("#psytest > div > div > main > div > div.section.single > div > footer > button.forward-button.btn.btn-primary.spark-icon-btn.rounded-circle.btn-lg");
    }

    private void ClickBigGreenContinueBtn()
    {
        Browser.ClickByCss(
            "#psytest > div > div > main > div > div.section.single.candy-screen > div > footer > button.btn.btn-primary.continue");
    }
    private void ClickIWillAnswerItLater()
    {
        Browser.ClickByCss("#psytest > div > div > main > div > div.section.single > div > footer > span");
    }
}