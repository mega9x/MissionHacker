using MissionHacker.ConfigHelper;
using Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ConstStr;

namespace InfoGen
{
    class Person
    {
        public static List<USAddressModel> USAddress = JsonSerializer.Deserialize<IEnumerable<USAddressModel>>(File.ReadAllText(ConfigPath.US_ADDRESS_DATA_PATH)).ToList();
        public static USAddressModel GetRandomUSAddress()
        {
            return USAddress[RandomNumberGenerator.GetInt32(0, USAddress.Count)];
        }
    }
}
