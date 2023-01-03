// See https://aka.ms/new-console-template for more information

using System.Text.Json;
using MissionHacker.ConfigHelper;
using MissionHandler;
using Models;
using Models.ConstStr;

Console.WriteLine("Hello, World!");
var config = Config.Instance;
var missionLoader = new MissionLoader();
await missionLoader.LoadMission();
foreach (var l in missionLoader.MissionList)
{
    Console.WriteLine(l.Platform);
}
