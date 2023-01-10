using System.Text;
using System.Text.Json;
using Models.BitBrowserApiModels;
using Models.BitBrowserApiModels.Reponse;

namespace BitBrowser;

public class Api
{
    public static async Task<ResponseBase<OpenBrowserResponse>> OpenBrowser(string id, string api)
    {
        var httpClient = new HttpClient();
        var browserPara = new OpenBrowser()
        {
            Id = id
        };
        var data = new StringContent(JsonSerializer.Serialize(browserPara), Encoding.UTF8, "application/json");
        var response = await httpClient.PostAsync(api + "/browser/open", data);
        var resStr = await response.Content.ReadAsStringAsync();
        Console.WriteLine(resStr);
        var resObj = JsonSerializer.Deserialize<ResponseBase<OpenBrowserResponse>>(resStr);
        if (resObj is null)
        {
            throw new Exception("Failed to open browser");
        }
        return resObj;
    }
}