using System.Text.Json;
using Models;
using Models.ConstStr;
using Models.Data;

namespace MissionHacker.ConfigHelper;

public class Config
{
    public General? General { get; private set; }
    public static Config Instance = new Lazy<Config>(() => new Config()).Value;
    public List<USAddressModel> USAddressModels;
    private Config()
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
        if (!File.Exists(ConfigPath.MAIL_POOL))
        {
            File.Create(ConfigPath.MAIL_POOL).Close();
            File.WriteAllText(ConfigPath.MAIL_POOL ,$"邮箱: xx@outlook.com | 密码: xxxxxx");
        }

        if (!File.Exists(ConfigPath.CONFIG_MAIL_USED))
        {
            File.Create(ConfigPath.CONFIG_MAIL_USED).Close();
        }
        if (!File.Exists(ConfigPath.CONFIG_GENERAL))
        {
            var str = JsonSerializer.Serialize(new General());
            File.Create(ConfigPath.CONFIG_GENERAL).Close();
            File.WriteAllText(ConfigPath.CONFIG_GENERAL, str);
        }
        General = JsonSerializer.Deserialize<General>(File.ReadAllText(ConfigPath.CONFIG_GENERAL));
        USAddressModels = JsonSerializer.Deserialize<IEnumerable<USAddressModel>>(File.ReadAllText(ConfigPath.US_ADDRESS_DATA_PATH)).ToList();
    }
    public Config SaveBitBrowserConfig(string link, string id)
    {
        General.BitApi = link;
        General.BitBrowserId = id;
        File.Delete(ConfigPath.CONFIG_GENERAL);
        var str = JsonSerializer.Serialize(General);
        File.Create(ConfigPath.CONFIG_GENERAL).Close();
        File.WriteAllText(ConfigPath.CONFIG_GENERAL, str);
        return this;
    }
}