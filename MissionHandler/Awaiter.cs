using MissionHacker.ConfigHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MissionHandler
{
    public class Awaiter
    {
        public static Task GetDelay()
        {
            return Task.Delay(RandomNumberGenerator.GetInt32(Config.Instance.MissionHackerConfig!.General.MinWait, Config.Instance.MissionHackerConfig!.General.MaxWait));
        }
    }
}
