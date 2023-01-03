using System.Text.Json;
using Models;
using Models.ConstStr;

namespace MissionHacker.ConfigHelper;

public class Config
{
    public General? General { get; private set; }
    public Config()
    {
        if (!Directory.Exists(ConfigPath.CONFIG_ROOT))
        {
            Directory.CreateDirectory(ConfigPath.CONFIG_ROOT);
        }
        if (!File.Exists(ConfigPath.CONST_MISSION))
        {
            var str = JsonSerializer.Serialize(new List<MissionConfigModel>()
            {
                new MissionConfigModel
                {
                    Area = "",
                    MinTimes = 1,
                    MaxTimes = 2,
                    Keyword = "",
                }
            });
            File.Create(ConfigPath.CONST_MISSION).Close();
            File.WriteAllText(ConfigPath.CONST_MISSION, str);
        }
        if (!File.Exists(ConfigPath.MailPool))
        {
            File.Create(ConfigPath.MailPool).Close();
            File.WriteAllText(ConfigPath.MailPool ,$"邮箱: xx@outlook.com | 密码: xxxxxx");
        }

        if (!File.Exists(ConfigPath.CONFIG_GENERAL))
        {
            var str = JsonSerializer.Serialize(new General());
            File.Create(ConfigPath.CONFIG_GENERAL).Close();
            File.WriteAllText(ConfigPath.CONFIG_GENERAL, str);
        }
        General = JsonSerializer.Deserialize<General>(File.ReadAllText(ConfigPath.CONFIG_GENERAL));
    }
}