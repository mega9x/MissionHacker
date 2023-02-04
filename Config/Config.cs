using System.Text.Json;
using ConstStr;
using Models.Config.MissionHacker;
using Models.Data;
using Tomlyn;

namespace MissionHacker.ConfigHelper;

public class Config
{
    public MissionHackerConfigRoot? MissionHackerConfig { get; private set; }
    public static readonly Config Instance = new Lazy<Config>(() => new Config()).Value;
    private Config()
    {
        if (!Directory.Exists(ConfigPath.CONFIG_ROOT))
        {
            Directory.CreateDirectory(ConfigPath.CONFIG_ROOT);
        }
        // 白名单任务
        // 要合并为 Root
        if (!File.Exists(ConfigPath.ConstMission))
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
            File.Create(ConfigPath.ConstMission).Close();
            File.WriteAllText(ConfigPath.ConstMission, str);
        }
        if (!File.Exists(ConfigPath.MailPool))
        {
            File.Create(ConfigPath.MailPool).Close();
            File.WriteAllText(ConfigPath.MailPool, $"邮箱: xx@outlook.com | 密码: xxxxxx");
        }
        if (!File.Exists(ConfigPath.ConfigMailUsed))
        {
            File.Create(ConfigPath.ConfigMailUsed).Close();
        }
        // 尝龟
        // 要合并为 Root
        if (!File.Exists(ConfigPath.ConfigGeneral))
        {
            var str = Toml.FromModel(new MissionHackerConfigRoot());
            File.Create(ConfigPath.ConfigGeneral).Close();
            File.WriteAllText(ConfigPath.ConfigGeneral, str);
        }
        if (!File.Exists(ConfigPath.ManualBlockList))
        {
            File.Create(ConfigPath.ManualBlockList).Close();
        }
        if (!Directory.Exists(ConfigPath.LOG_ROOT))
        {
            Directory.CreateDirectory(ConfigPath.LOG_ROOT);
        }
        MissionHackerConfig = Toml.ToModel<MissionHackerConfigRoot>(File.ReadAllText(ConfigPath.ConfigGeneral));
    }
    public Config SaveBitBrowserConfig(string link, string id)
    {
        MissionHackerConfig.General.BitApi = link;
        MissionHackerConfig.General.BitBrowserId = id;
        Save();
        return this;
    }
    public Config SaveId(string id)
    {
        MissionHackerConfig.General.Id = id;
        Save();
        return this;
    }
    public Config SaveProxyApi(string ip)
    {
        MissionHackerConfig.General.ProxyApi = ip;
        Save();
        return this;
    }
    private Config Save()
    {
        File.Delete(ConfigPath.ConfigGeneral);
        var str = Toml.FromModel(MissionHackerConfig);
        File.Create(ConfigPath.ConfigGeneral).Close();
        File.WriteAllText(ConfigPath.ConfigGeneral, str);
        return this;
    }
}