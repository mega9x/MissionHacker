using MissionHacker.ConfigHelper;
using Models.ConstStr;
using Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace InfoGen
{
    class Person
    {
        public static USAddressModel GetRandomUSAddress()
        {
            return Config.Instance.USAddressModels[RandomNumberGenerator.GetInt32(0, Config.Instance.USAddressModels.Count)];
        }
    }
}
