using System.Text.Json.Serialization;

namespace Models;

public class BackendModel
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = "";
    [JsonPropertyName("platform")]
    public string Platform { get; set; } = "";
    [JsonPropertyName("link")]
    public string Link { get; set; } = "";
    [JsonPropertyName("code")]
    public string Code { get; set; } = "";
}