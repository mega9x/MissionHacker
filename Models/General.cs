namespace Models;

public class General
{
    public string Proxy = "127.0.0.1:40000";

    public string ProxyApi =
        "http://192.168.101.3:9049/v1/ips?num=1&country=国家&state=all&city=all&zip=all&t=txt&port=40000&isp=all&start=&end=";

    public string BitApi =
        "http://127.0.0.1:62105";

    public string BitBrowserId = "227b02a2613a45479aec5cdf5276da1a";
    public string ApiReplacement = "国家";
}