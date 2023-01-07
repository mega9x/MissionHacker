using System.Collections.ObjectModel;
using System.Security.Cryptography;
using ChanceNET;
using Crawler;
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
        var tall = sex == 0 ? RandomNumberGenerator.GetInt32(169, 190) : RandomNumberGenerator.GetInt32(150, 180);
        var random = new RandomGen();
        var nickname = random.GetRandomName();
        var genderSelectorOne = "";
        var genderSelectorOne_LookingFor = "";
        var genderSelectorTwo = "";
        var genderSelectorTwo_LookingFor = "";
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
        // 打开年龄选择器
        var password = info.Mail.MailPd + RandomNumberGenerator.GetInt32(0, 9999);
        MainDriver.ExecuteJavaScript($"document.querySelector('{genderSelectorOne}').click()");
        MainDriver.ExecuteJavaScript($"document.querySelector('{genderSelectorOne_LookingFor}').click()");
        MainDriver.FindElement(By.CssSelector("#custom-select")).Click();
        MainDriver.FindElements(By.CssSelector(".ageRange-list > div"))[ageSelectorIndex].Click();
        MainDriver.FindElement(By.CssSelector("#emailaddy")).SendKeys(info.Mail.MailName);
        MainDriver.FindElement(By.CssSelector("#legal")).Click();
        MainDriver.FindElement(By.CssSelector("#submit-btn-01")).Click();
        await Wait();
        MainDriver.FindElement(By.CssSelector("#password")).SendKeys(password);
        await Wait(1000);
        MainDriver.FindElement(By.CssSelector("#passwordRepeat")).SendKeys(password);
        await Wait();
        Browser.ClickByCss("#submit-btn");
        MainDriver.SwitchTo().Frame(Browser.QuerySelector("#h-captcha-wrapper > iframe"));
        Browser.ClickByCss("#anchor-state");
        MainDriver.SwitchTo().ParentFrame();
        await Wait();
        Browser.ClickByCss(
            "#psytest > div > div > main > div > div.section.single.candy-screen > div > footer > button.btn.btn-primary.continue");
        await Wait();
        Browser.ClickByCss(genderSelectorTwo);
        await Wait();
        await Wait(1200);
        Browser.ClickByCss(genderSelectorTwo_LookingFor);
        await Wait();
        await Wait(1200);
        SelectAnswers(0, 4);
        await Wait();
        await Wait(1200);
        SelectAnswers(0, 7);
        await Wait();
        await Wait(1200);
        SelectAnswers(0, 7);
        var tallInput = Browser.QuerySelector(
                "#psytest > div > div > main > div > div.section.single > div > div > div > div.spark-form-group.mb-0 > div > div.spark-form-group.body-height-form-group > div > div > div > label > input");
        tallInput.SendKeys(Keys.Control + "a");
        tallInput.SendKeys(Keys.Backspace);
        tallInput.Clear();
        tallInput.SendKeys(tall.ToString());
        Browser.ClickByCss(
            "#psytest > div > div > main > div > div.section.single > div > footer > button.forward-button.btn.btn-primary.spark-icon-btn.rounded-circle.btn-lg");
        await Wait();
        await Wait(1200);
        SelectAnswers(0, 7);
        await Wait();
        await Wait(1200);
        SelectPic(0, 4);
        await Wait();
        await Wait(1200);
        // Which picture is more appealing to you?
        SelectPic(0, 4);
        await Wait();
        await Wait(1200);
        // Which ethnic group do you belong to?
        SelectAnswers(0, 7);
        // Do you have ethnic preferences
        await Wait();
        await Wait(1200);
        SelectItem(0, 8, 0);
        await Wait();
        await Wait(1200);
        SelectAnswers(0, 7);
        await Wait();
        await Wait(1200);
        SelectAnswers(0, 7);
        // How important is it to you that your partner has an attractive appearance?
        await Wait();
        await Wait(1200);
        SelectAnswers(0, 7);
        // Which of the following best describes your beliefs?
        await Wait();
        await Wait(1200);
        SelectAnswers(0, 9);
        // Which religious beliefs would you be open to?
        await Wait();
        await Wait(1200);
        SelectItem(0, 10, 0);    
        // How important is your partner’s religion or spirituality to you?
        await Wait();
        await Wait(1200);
        SelectAnswers(0, 9);
        // 中场休息
        await Wait();
        await Wait(1200);
        Browser.ClickByCss(
            "#psytest > div > div > main > div > div.section.single.candy-screen > div > footer > button.btn.btn-primary.continue");
        // Which role would you like a partner to fulfill the most?
        await Wait();
        await Wait(1200);
        SelectAnswers(0, 5);
        await Wait();
        await Wait(1200);
        SelectPic(0, 4);
        await Wait();
        await Wait(1200);
        SelectAnswers(0, 5);
        for (var i = 1; i <= 5; i++)
        {
            await Wait();
            await Wait(1200);
            SelectAnswers(0, 4);
        }
        await Wait();
        await Wait(1200);
        SelectItem(0, 32, 0);
        Browser.ClickByCss(
            "#psytest > div > div > main > div > div.section.single.candy-screen > div > footer > button.btn.btn-primary.continue");
        await Wait();
        await Wait(1200);
        SelectAnswers(0, 7);
        await Wait();
        await Wait(1200);
        SelectAnswers(0, 7);
        await Wait();
        await Wait(1200);
        SelectAnswers(0, 4);
        await Wait();
        await Wait(1200);
        SelectAnswers(0, 4);
        // How much of your weekly time would you like to share with your partner?
        await Wait();
        await Wait(1200);
        SelectPic(0, 4);
        await Wait();
        await Wait(1200);
        SelectPic(0, 4);
        await Wait();
        await Wait(1200);
        SelectPic(0, 4);
        await Wait();
        await Wait(1200);
        SelectAnswers(0, 7);
        // Imagine you are moving to a new city. How do you settle in?
        await Wait();
        await Wait(1200);
        SelectAnswers(0, 4);
        // I make time for others.
        await Wait();
        await Wait(1200);
        SelectAnswers(0, 7);
        // I like order and consistency.
        await Wait();
        await Wait(1200);
        SelectAnswers(0, 7);
        // I consider myself to be ambitious.
        await Wait();
        await Wait(1200);
        SelectAnswers(0, 7);
        // I pay attention to detail.
        await Wait();
        await Wait(1200);
        SelectAnswers(0, 7);
        // I get upset easily.
        await Wait();
        await Wait(1200);
        SelectAnswers(0, 7);
        // I can handle a great deal of information.
        await Wait();
        await Wait(1200);
        SelectAnswers(0, 7);
        // I like to do nice things for others.
        await Wait();
        await Wait(1200);
        SelectAnswers(0, 7);
        // I consider myself to be protective
        await Wait();
        await Wait(1200);
        SelectAnswers(0, 7);
        // I consider myself to be understanding.
        await Wait();
        await Wait(1200);
        SelectAnswers(0, 7);
        // I have little patience.
        await Wait();
        await Wait(1200);
        SelectAnswers(0, 7);
        // I consider myself to be responsible.
        await Wait();
        await Wait(1200);
        SelectAnswers(0, 7);
        // I consider myself to be well-informed.
        await Wait();
        await Wait(1200);
        SelectAnswers(0, 7);
        // I consider myself to be creative.
        await Wait();
        await Wait(1200);
        SelectAnswers(0, 7);
        // I accomplish a lot.
        await Wait();
        await Wait(1200);
        SelectAnswers(0, 7);
        // 决赛
        await Wait(1200);
        await Wait();
        MainDriver.FindElement(By.CssSelector(
                "#psytest > div > div > main > div > div.section.single.candy-screen > div > footer > button.btn.btn-primary.continue"))
            .Click();
        await Wait(1200);
        await Wait();
        SelectPic(0, 4);
        await Wait();
        await Wait(1200);
        SelectPic(0, 4);
        await Wait();
        await Wait(1200);
        SelectPic(0, 4);
        // I like a great deal of variety
        await Wait();
        await Wait(1200);
        SelectAnswers(0, 7);
        await Wait();
        await Wait(1200);
        SelectAnswers(0, 7);
        await Wait();
        await Wait(1200);
        SelectAnswers(0, 7);
        await Wait();
        await Wait(1200);
        SelectAnswers(0, 7);
        await Wait();
        await Wait(1200);
        SelectAnswers(0, 7);
        await Wait();
        await Wait(1200);
        SelectAnswers(0, 7);
        await Wait();
        await Wait(1200);
        SelectAnswers(0, 7);
        await Wait();
        await Wait(1200);
        SelectAnswers(0, 7);
        await Wait();
        await Wait(1200);
        SelectAnswers(0, 7);
        await Wait();
        await Wait(1200);
        SelectAnswers(0, 7);
        await Wait();
        await Wait(1200);
        SelectItem(0, 32, 4);
        Browser.ClickByCss(
            "#psytest > div > div > main > div > div.section.single > div > footer > button.forward-button.btn.btn-primary.spark-icon-btn.rounded-circle.btn-lg");
        await Wait();
        await Wait(1200);
        Browser.ClickByCss("#psytest > div > div > main > div > div.section.single > div > footer > span");
        await Wait();
        await Wait(1200);
        SelectItem(0, 39, 0); 
        Browser.ClickByCss(
            "#psytest > div > div > main > div > div.section.single > div > footer > button.forward-button.btn.btn-primary.spark-icon-btn.rounded-circle.btn-lg");
        await Wait();
        await Wait(1200);
        Browser.ClickByCss("#psytest > div > div > main > div > div.section.single > div > footer > span");
        await Wait();
        await Wait(1200);
        Browser.ClickByCss("#psytest > div > div > main > div > div.section.single > div > footer > span");
        await Wait();
        await Wait(1200);
        SelectAnswers(0, 5);
        await Wait();
        await Wait(1200);
        SelectAnswers(0, 5);
        await Wait();
        await Wait(1200);
        SelectAnswers(0, 7);
        await Wait();
        await Wait(1200);
        SelectAnswers(0, 7);
        await Wait();
        await Wait(1200);
        SelectAnswers(0, 5);
        await Wait();
        await Wait(1200);
        SelectAnswers(0, 5);
        await Wait();
        await Wait(1200);
        SelectAnswers(0, 7);
        await Wait();
        await Wait(1200);
        SelectAnswers(0, 4);
        await Wait();
        await Wait(1200);
        SelectAnswers(0, 3);
        await Wait();
        await Wait(1200);
        SelectAnswers(0, 3);
        // Please enter your first name
        await Wait();
        await Wait(1200);
        Browser.SendKeysByCss(
            "#psytest > div > div > main > div > div.section.single > div > div > div > div > div > div > div > label > input",
            nickname);
        await Wait();
        await Wait(1200);
        Browser.ClickByCss(
            "#psytest > div > div > main > div > div.section.single > div > footer > button.forward-button.btn.btn-primary.spark-icon-btn.rounded-circle.btn-lg");
        await Wait();
        await Wait(1200);
        Browser.SendKeysByCss(
            "#psytest > div > div > main > div > div.section.single > div > div > div.location-answer > div.spark-form-group > div > div > div > label > input",
            Browser.GetIPData().CurrentZip);
        await Wait();
        await Wait(1200);
        Browser.ClickByCss(
        "body > div.spark-autocomplete-suggestions.spark-autocomplete-bottom > div.spark-autocomplete-suggestion.to-select");
        Browser.ClickByCss(
            "#psytest > div > div > main > div > div.section.single > div > footer > button.forward-button.btn.btn-primary.spark-icon-btn.rounded-circle.btn-lg");
        await Wait();
        await Wait(1200);
        SelectAnswers(0, 5);
        await Wait();
        await Wait(1200);
        SelectAnswers(0, 7);
        Browser.ClickByCss(
            "#psytest > div > div > main > div > div.section.single > div > div > div > div.d-flex > div.date-input-field.month-field > div > div > div.spark__value-container.spark__value-container--has-value.css-1hwfws3");
        var date = Date.GenRandomDate();
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
        Browser.ClickByCss(
            "#psytest > div > div > main > div > div.section.single > div > footer > button.forward-button.btn.btn-primary.spark-icon-btn.rounded-circle.btn-lg");
        await Wait();
        await Wait(1200);
        SelectAnswers(0, 7);
        await Wait();
        await Wait(1200);
        Browser.SendKeysByCss(
            "#psytest > div > div > main > div > div.section.single > div > div > div > div > div > div.spark__value-container.css-1hwfws3",
            random.GetRandomProfession());
        Browser.ClickByCss(
            "#psytest > div > div > main > div > div.section.single > div > footer > button.forward-button.btn.btn-primary.spark-icon-btn.rounded-circle.btn-lg");
        await Wait();
        await Wait(1200);
        SelectAnswers(0, 7);
        await Wait();
        await Wait(1200);
        SelectAnswers(0, 7);
        await Wait();
        await Wait(1200);
        MainDriver.FindElements(
                    By.CssSelector(
                        "#psytest > div > div > main > div > div.section.single > div > div > div > div > div"))        
                [RandomNumberGenerator.GetInt32(0, 11)].Click();
        await Wait();
        await Wait(1200);
        Browser.ClickByCss(
            "#psytest > div > div > main > div > div.section.candy-screen.single > div > footer > button.btn.btn-primary.continue");
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
        // MailChrome.ClickLatestFocused()
        return this;
    }
    private IWebElement SelectAnswers(int min, int max)
    {
        var selector = MainDriver.FindElements(By.CssSelector("#psytest > div > div > main > div > div.section.single > div > div > div > div"))
            [RandomNumberGenerator.GetInt32(min, max)].FindElement(By.CssSelector("input"));
        selector.Click();
        return selector;
    }
    private IWebElement SelectPic(int min, int max)
    {
        var selector =
            MainDriver.FindElements(
                    By.CssSelector(
                        "#psytest > div > div > main > div > div.section.single > div > div > div > div > label"))
                [RandomNumberGenerator.GetInt32(min, max)];
        selector.Click();
        return selector;
    }
    private IList<IWebElement> SelectItem(int min, int max, int selectMin)
    {
        var times = RandomNumberGenerator.GetInt32(selectMin + 2, 8);
        List<IWebElement> list = new();
        for (int i = 0; i < times; i++)
        {
            var selector =
                MainDriver.FindElements(
                    By.CssSelector(
                        "#psytest > div > div > main > div > div.section.single > div > div > div > div > div"))        
                    [RandomNumberGenerator.GetInt32(min, max)];
            list.Add(selector);
            selector.Click();
        }
        return list;
    }
}