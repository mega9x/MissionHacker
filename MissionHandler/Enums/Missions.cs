﻿using MissionHandler.MissionFactory;

namespace Models.Enums;

public static class Missions
{
    public static readonly Dictionary<string, IMissionHandler> AllMissionsKeywordSupported = new()
    {
        // "elitesingles",
        {"flirt-dating", new FlirtDating()},
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