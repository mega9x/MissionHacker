using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace Models;

public class BackendModel
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = "";
    [JsonPropertyName("platform")]
    public string Platform { get; set; } = "";
    [JsonPropertyName("domain")]
    public string Domain { get; set; } = "";
    [JsonPropertyName("link")]
    public string Link { get; set; } = "";
    [JsonPropertyName("code")]
    public string Code { get; set; } = "";
    [JsonPropertyName("mid")]
    public string MId { get; set; } = "";
    [JsonPropertyName("times")]
    public string Times { get; set; } = "";
    [JsonPropertyName("launched")]
    public string Launched { get; set; } = "";
    [JsonPropertyName("area")]
    public string Area { get; set; } = "";
    [JsonPropertyName("price")]
    public string Price { get; set; } = "";

    public string GetLink()
    {
        var uriRegex = new Regex("(https?|ftp|file)://[-A-Za-z0-9+&@#/%?=~_|!:,.;]+[-A-Za-z0-9+&@#/%=~_|]");
        if (Link is not null && Link.Length > 0)
        {
            return Link;
        }
        else
        {
            return uriRegex.Match(Link).Value;
        }
    }

}