using System.Security.Cryptography;
using MissionHandler.MissionFactory.AbstractHandler;
using Models.Data;
using Models.Enum;
using OpenQA.Selenium;
using Utils.InfoGen;

namespace MissionHandler.MissionFactory;

public class Christianmingle: AbstractMissionHandler
{
    public SinglePerson Person { get; set; }
    public override async Task<IMissionHandler> RunAsync()
    {
        Person = await Utils.InfoGen.PersonFactory.GenPerson();
        var photoFile = await Person.GetPhoto();
        Browser.Timeout = TimeSpan.FromSeconds(60);
        Browser.ClickByCss("[class='btn btn-primary btn-block']");
        await Wait(1000);
        Browser.ClickByJsCss(Person.Sex == Sex.Female ? "#genderFemale" : "#genderMale");
        Browser.SendKeysByCss("#register_email", info.Mail.MailName);
        Browser.SendKeysByCss("#password", info.Mail.MailPd);
        Browser.ClickByCss("[class='btn btn-lg btn-primary btn-block']");
        await Wait();
        Browser.SendKeysByCss("#register_firstname", Person.Firstname);
        Browser.SendKeysByCss("#register_lastname", Person.Lastname);
        // 选择生日
        Browser.SelectByCss("#register_birthday_month", Person.Birthmonth);
        Browser.SelectByCss("#register_birthday_day", Person.Birthday);
        Browser.SelectByCss("#register_birthday_year", Person.Birthyear.ToString());
        // 同意条款
        Browser.ClickByJsCss("#register_terms");
        Browser.SendKeysByCss("#location", IpData.CurrentZip);
        // 点击下一步
        Browser.ClickByCss("[class='pac-container pac-logo'] .pac-item");
        // 点击 captcha 
        Browser.ClickEleOfIframe("#g-recaptcha iframe", ".recaptcha-checkbox-border");
        await Wait(3000);
        Browser.SendKeysByCss("[type='file']", photoFile.FullName);
        // 上传文件 
        await Submit();
        // 用户名显示
        var radios = MainDriver.FindElements(By.CssSelector(".radio-container"));
        switch (GetRandomFixed(0, 1))
        {
            case 0:
                radios[0].Click(); break;
            case 1:
                radios[1].Click();
                Browser.SendKeysByCss("#createusername_change_display_name", Person.Nickname);
                break;
        }
        await Submit();
        // 设置身高
        var dragable = Browser.QuerySelector(".rc-slider-handle").End().Queryed;

        await Wait();
        return this;
    }

    public async Task Submit()
    {
        Browser.ClickByCss("[type='submit']");
    }
}