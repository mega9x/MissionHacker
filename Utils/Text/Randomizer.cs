using System.Globalization;
using Utils.Randomizer;

namespace Utils.Text;

public class Randomizer
{
    public static string RandomStyle(string str)
    {
        var result = str;
        Function.ChanceInvokeOrDefault(() => {
            Function.ChanceInvoke(() => {
                result = str.ToUpper();
            }, 5);
        }, () => Function.ChanceInvoke(() => {
            result = str.ToLower();
        }, 5), 2);
        return result;
    }
    public static string RandomTitleStyle(string str)
    {
        var result = str;
        Function.ChanceInvoke(() => {
            var culture = new CultureInfo("en-US").TextInfo;
            result = culture.ToTitleCase(str);
        }, 5);
        Function.ChanceInvoke(() => {
            result = str.ToUpper();
        }, 10);
        return result;
    }
}