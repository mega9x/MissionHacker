using ChanceNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace MissionHandler.Utils
{
    public class StrGen
    {
        public static string GetPassword()
        {
            var chance = new Chance();
            return chance.String(15);
        }
    }
}
