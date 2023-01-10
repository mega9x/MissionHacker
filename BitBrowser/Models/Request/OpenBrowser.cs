using System.Text.Json.Serialization;

namespace Models.BitBrowserApiModels;

public class OpenBrowser
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = "";
    [JsonPropertyName("args")]
    public string[] Args { get; set; } = Array.Empty<string>();
    [JsonPropertyName("loadExtensions")]
    public bool LoadExtensions = true;
}