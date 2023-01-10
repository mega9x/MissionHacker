using MissionHacker.ConfigHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MissionHandler
{
    class Awaiter
    {
        public static Task GetDelay()
        {
            return Task.Delay(RandomNumberGenerator.GetInt32(Config.Instance.General.MinWait, Config.Instance.General.MaxWait));
        }
    }
}
