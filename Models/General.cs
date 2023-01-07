using Models.ConstStr;

namespace Models;

public class General
{
    public string Proxy { get; set; } = "127.0.0.1:40000";

    public string ProxyApi { get; set; } =
        "http://192.168.101.3:9049/v1/ips?num=1&country=国家&state=all&city=all&zip=all&t=txt&port=40000&isp=all&start=&end=";

    public string BitApi { get; set; } = "http://127.0.0.1:62105";

    public string BitBrowserId { get; set; } = "227b02a2613a45479aec5cdf5276da1a";
    public string ApiReplacement { get; set; } = "国家";
    public int MaxWait { get; set; } = 10000;
    public int MinWait { get; set; } = 2000;
}