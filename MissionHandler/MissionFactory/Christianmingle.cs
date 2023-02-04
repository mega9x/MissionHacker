using Models.Enum;

namespace MissionHandler.MissionFactory;

public class Christianmingle: AbstractMissionHandler
{
    public override async Task<IMissionHandler> RunAsync()
    {
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
        Browser.ClickByCss("[class='pac-container pac-logo'] .pac-item");
        // 点击 captcha 
        Browser.ClickEleOfIframe("#g-recaptcha iframe", ".recaptcha-checkbox-border");
        await Wait(3000);
        await Wait();
        return this;
    }
}