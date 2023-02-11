namespace ConstStr;

public class ConfigPath
{
    #region PathRoot
    public const string CONFIG_ROOT = "./配置";
    public const string PHOTO_ROOT = "./图片";
    public const string DATA_ROOT = "./data";
    public const string Tmp = "./tmp";
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
    #region AddressData
    public const string US_ADDRESS_DATA_PATH = $"{DATA_ROOT}/usaddress.json";
    #endregion
    #region MovieData
    public const string NetflixDbPath = $"{DATA_ROOT}/movies/netfilx.csv";
    public const string TmdbPath = $"{DATA_ROOT}/movies/tmdb.csv";
    public const string AnimePath = $"{DATA_ROOT}/movies/anime.csv";
    #endregion
    #region MovieGenresData
    public const string MovieTvGenresPath = $"{DATA_ROOT}/movies/movie_tv_genres.csv";
    public const string AnimeGenresPath = $"{DATA_ROOT}/movies/tmdb.csv";
    #endregion
    #region MovieVideoSites
    public const string MovieSitePath = $"{DATA_ROOT}/movies/websites.txt";
    #endregion
    #region BookData
    public const string BooksPath = $"{DATA_ROOT}/books/books.csv";
    #endregion
    #region BookGenresData
    public const string BookGenresPath = $"{DATA_ROOT}/books/book_genres.txt";
    #endregion
    #region BookArtists
    public const string BookArtistsPath = $"{DATA_ROOT}/books/book_artists.txt";
    #endregion
}