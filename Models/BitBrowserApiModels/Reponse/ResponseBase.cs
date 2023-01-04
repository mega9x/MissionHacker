using System.Text.Json.Serialization;

namespace Models.BitBrowserApiModels.Reponse;

public class ResponseBase<T>
{
    [JsonPropertyName("success")]
    public bool Success { get; set; }
    [JsonPropertyName("data")]
    public T Data { get; set; }
}