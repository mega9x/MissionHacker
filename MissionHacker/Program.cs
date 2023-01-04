// See https://aka.ms/new-console-template for more information

using System.Net;
using System.Text.Json;
using Crawler;
using MissionHacker.ConfigHelper;
using MissionHandler;
using Models;
using Models.ConstStr;
using OpenQA.Selenium.Support.Extensions;

Console.WriteLine("Hello, World!");
var config = Config.Instance;
var missionLoader = new MissionLoader();
await missionLoader.LoadMission();

while (true)
{
    foreach (var l in missionLoader.MissionList)
    {
        Console.WriteLine(l.Keyword);
    }

    return;
}


IBrowser browser = new Chrome();
var driver = await browser.GetDriver();
driver.ExecuteJavaScript("window.open(\"https://www.baidu.com\")");
