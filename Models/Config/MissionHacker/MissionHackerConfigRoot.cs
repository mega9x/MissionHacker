using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Config.MissionHacker
{
    public class MissionHackerConfigRoot
    {
        public General General { get; set; } = new();
        public MissionConfigModel MissionConfigModel { get; set; } = new();
        public PhotoApi PhotoApi { get; set; } = new();
    }
}
