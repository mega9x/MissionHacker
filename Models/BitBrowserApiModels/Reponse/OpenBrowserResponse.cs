using System.Text.Json.Serialization;

namespace Models.BitBrowserApiModels.Reponse;

public class OpenBrowserResponse
{
    [JsonPropertyName("ws")]
    public string Ws { get; set; }
    [JsonPropertyName("http")]
    public string Http { get; set; }
}