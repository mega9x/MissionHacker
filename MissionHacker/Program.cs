// See https://aka.ms/new-console-template for more information

using System.Net;
using System.Text.Json;
using System.Text.RegularExpressions;
using Crawler;
using MissionHacker.ConfigHelper;
using MissionHandler;
using Models;
using Models.Enums;
using OpenQA.Selenium.Support.Extensions;

var config = Config.Instance;
var missionLoader = new MissionLoader();
await missionLoader.LoadMission();
MailProvider.MailProvider mail = MailProvider.MailProvider.Instance;
try
{
    while (true)
    {
        var m = new Mail("");
        try
        {
            m = new Mail(await mail.GetMail());
        }
        catch(Exception e)
        {
            Console.WriteLine("无法读取邮箱或邮箱已用完");
            Console.WriteLine(e.ToString());
        }
        var mailBrowser = new MailChrome();
        var mainBrowser = new Chrome();
        var area = "";

        var allHandled = true;
        var uriRegex = new Regex("(https?|ftp|file)://[-A-Za-z0-9+&@#/%?=~_|!:,.;]+[-A-Za-z0-9+&@#/%=~_|]");
        foreach (var l in missionLoader.MissionList)
        {
            allHandled = allHandled && l.IsAllHandled();
            if (l.IsAllHandled())
            {
                continue;
            }
            if (l.Area != area)
            {
                mainBrowser.ChangeIp(l.Area);
                area = l.Area;
            }
            Console.WriteLine(l.Keyword);
            var codes = l.GetCode();
            if (codes is null) continue;
            var strLink = uriRegex.Match(l.GetCode()!.Code).Value;
            var link = new Links
            {
                Link = strLink,
                Domain = codes.Domain
            };

            var info = new MissionInfo()
            {
                Mail = m,
                Links = link.Link.Length <= 0 ? null : link,
            };
            if (!Missions.AllMissionsKeywordSupported.ContainsKey(l.Keyword))
            {
                l.Lock();
                continue;
            }
            var handler = Missions.AllMissionsKeywordSupported[l.Keyword];
            // 启动任务处理器
            await (await (await handler.SetBrowser(mainBrowser)).SetMailBrowser(mailBrowser)).SetInfo(info).RunAsync();
            l.NextCode();
        }
        mainBrowser.ChangeIp("US");
        (await mainBrowser.GetDriver()).Quit();
        if (allHandled)
        {
            break;
        }
        return;
    }
}
catch (Exception e)
{
    Console.WriteLine(e.ToString());
    Console.ReadLine();
}
