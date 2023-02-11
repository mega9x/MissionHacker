using System.Security.Cryptography;

namespace Utils.Randomizer;

public class Function
{
    public static async Task ChanceInvoke(Action action, int chance)
    {
        if (RandomNumberGenerator.GetInt32(0, chance) == 1)
        {
            action();
        }
    }
    public static async Task ChanceInvokeOrDefault(Action action, Action defaultAction, int chance)
    {
        if (RandomNumberGenerator.GetInt32(0, chance) == 1)
        {
            action();
        }
        else
        {
            defaultAction();
        }
    }
    public static async Task ChanceInvoke(Action action)
    {
        if (RandomNumberGenerator.GetInt32(0, 2) == 1)
        {
            action();
        }
    }
    public static async Task LessChanceInvoke(Action action)
    {
        if (RandomNumberGenerator.GetInt32(0, 4) == 1)
        {
            action();
        }
    }
}