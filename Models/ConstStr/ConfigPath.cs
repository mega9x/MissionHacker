namespace Models.ConstStr;

public class ConfigPath
{
    public const string CONFIG_ROOT = "./配置";
    public const string CONFIG_GENERAL = $"{CONFIG_ROOT}/常规.json";
    public const string CONFIG_MAIL_USED = $"{CONFIG_ROOT}/用过的邮箱 (勿删).txt";
    public const string CONST_MISSION = $"{CONFIG_ROOT}/支持的任务.json";
    public const string MAIL_POOL = $"{CONFIG_ROOT}/邮箱池.txt";
    public const string BLOCK_LSIT = $"{CONFIG_ROOT}/今日黑名单date.txt";
    public const string SUPPORT_LIST = $"{CONFIG_ROOT}/任务关键词 (勿改).json";
}