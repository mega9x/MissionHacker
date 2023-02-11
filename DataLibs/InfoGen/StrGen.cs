using ChanceNET;

namespace DataLibs.InfoGen
{
    public class StrGen
    {
        public static string GetPassword()
        {
            var chance = new Chance();
            return chance.String(15);
        }
    }
}
