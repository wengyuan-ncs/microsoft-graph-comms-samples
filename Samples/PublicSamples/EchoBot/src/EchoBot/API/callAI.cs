
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

public class CallApiService
{
    private readonly HttpClient _httpClient;

    public CallApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> CallAiApiAsync(string url, string recognizedText, string sessionId)
    {
        var request = new ChatRequest
        {
            sessionId = sessionId,
            message = recognizedText
        };

        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        using var response = await _httpClient.PostAsync(url, content);
        response.EnsureSuccessStatusCode();

        var responseJson = await response.Content.ReadAsStringAsync();
        var aiResponse = JsonSerializer.Deserialize<ChatResponse>(responseJson);

        return aiResponse?.response ?? string.Empty;
    }
}


public class ChatRequest
{
    public string sessionId { get; set; }
    public string message { get; set; }
}

public class ChatResponse
{
    public string sessionId { get; set; }
    public string response { get; set; }
}
