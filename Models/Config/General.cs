namespace Models;

public class General
{
    public string Id { get; set; } = "mega9x";
    public string Proxy { get; set; } = "127.0.0.1:40000";
    public string ProxyApi { get; set; } = "http://127.0.0.1:9049";
    public string BitApi { get; set; } = "http://127.0.0.1:62105";
    public string BitBrowserId { get; set; } = "227b02a2613a45479aec5cdf5276da1a";
    public int MaxWait { get; set; } = 10000;
    public int MinWait { get; set; } = 2000;
}