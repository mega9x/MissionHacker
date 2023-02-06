using MissionHandler.MissionFactory;
using MissionHandler.MissionFactory.AutoSelectMission;

namespace MissionHandler.Enums;

public static class Missions
{
    public static readonly Dictionary<string, IMissionHandler> AllMissionsKeywordSupported = new()
    {
        {"elitesingles", new Elitesingles()},
        {"flirt-dating", new FlirtDating()},
        {"christianmingle", new Christianmingle()},
        {"eharmony", new Eharmony()},
        // "seniorsexmatch",
        // "hookups - soi", 
        // "searchingforsingles - doi",
        // "sweepsex - soi",
        // "Eharmony.com",
        // "battle arena",
        // "your smartlink",
        // "top moving quote"
    };
}