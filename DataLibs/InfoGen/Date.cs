using ChanceNET;

namespace DataLibs.InfoGen;

public class Date
{
    public static DateTime GenRandomDate()
    {
        var chance = new Chance();
        return chance.Date();
    }
}