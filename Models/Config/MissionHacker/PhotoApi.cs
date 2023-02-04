using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Config.MissionHacker
{
    public class PhotoApi
    {
        public string Base { get; set; } = "http://photo.api.ykstudio.pro";
        public string GetPhotoEndpoint { get; set; } = "/Photo/GetPhotoGroup";
    }
}
