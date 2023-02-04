namespace ConstStr;

public class ConfigPath
{
    #region PathRoot
    public const string CONFIG_ROOT = "./配置";
    public const string PHOTO_ROOT = "./图片";
    public const string DATA_ROOT = "./data";
    public const string LOG_ROOT = "./日志";
    #endregion
    #region Config
    public const string ManualBlockList = $"{CONFIG_ROOT}/黑名单.txt";
    public const string ConfigFile = $"设置.toml";
    public const string ConfigGeneral = $"{CONFIG_ROOT}/常规.toml";
    public const string ConfigMailUsed = $"{CONFIG_ROOT}/用过的邮箱 (勿删).txt";
    public const string ConstMission = $"{CONFIG_ROOT}/支持的任务.json";
    public const string MailPool = $"{CONFIG_ROOT}/邮箱池.txt";
    public const string BlockList = $"{CONFIG_ROOT}/今日已完成date.txt";
    public const string SupportList = $"{CONFIG_ROOT}/任务关键词 (勿改).json";
    #endregion
    #region log
    public const string HANDLED_MISSION_LOG = $"{LOG_ROOT}/日志date.txt";
    #endregion
    #region Data
    public const string US_ADDRESS_DATA_PATH = $"{DATA_ROOT}/usaddress.json";
    #endregion
}