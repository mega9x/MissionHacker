using MissionHandler.MissionFactory;

namespace Models.Enums;

public static class Missions
{
    public static readonly Dictionary<string, IMissionHandler> AllMissionsKeywordSupported = new()
    {
        {"elitesingles", new Elitesingles()},
        {"flirt-dating", new FlirtDating()},
        {"christianmingle", new Christianmingle()}
        // "seniorsexmatch",
        // "hookups - soi", 
        // "searchingforsingles - doi",
        // "sweepsex - soi",
        // "eharmony.com",
        // "battle arena",
        // "your smartlink",
        // "top moving quote"
    };
}