using System.Text;

namespace Utils.Text;

public static class Base64
{
    public static string DecodeBase64(this string b64)
    {
        var b64Byte = Convert.FromBase64String(b64);
        return Encoding.UTF8.GetString(b64Byte);
    }
}