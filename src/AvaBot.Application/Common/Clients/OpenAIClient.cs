using AvaBot.Application.Common.Models.Settings;
using Microsoft.Extensions.Options;
using Pgvector;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AvaBot.Application.Common.Clients;
public class OpenAIClient
{
    private readonly OpenAPISettings _openAPISettings;
    private readonly HttpClient _httpClient;

    public OpenAIClient(IOptions<OpenAPISettings> openAPISettings, HttpClient httpClient)
    {
        _openAPISettings = openAPISettings.Value;
        _httpClient = httpClient;
    }

    public async Task<EmbeddingsAPIResponse> CreateEmbeddings(string input)
    {
        var content = new EmbeddingsAPIRequest
        {
            Model = _openAPISettings.EmbeddingModel!,
            Input = input,
        };

        var serContent = JsonSerializer.Serialize(content);
        var request = new HttpRequestMessage(HttpMethod.Post, _openAPISettings.EmbeddingsBaseUrl)
        {
            Headers = {
                Authorization = new AuthenticationHeaderValue("Bearer", _openAPISettings.APIKey)
            },
            Content = new StringContent(serContent, Encoding.UTF8, "application/json")
        };

        var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseContentRead);

        var responseAsString = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<EmbeddingsAPIResponse>(responseAsString);
    }
}
public class EmbeddingsAPIRequest
{
    [JsonPropertyName("model")]
    public string Model { get; set; }

    [JsonPropertyName("input")]
    public string Input { get; set; }
}

public class EmbeddingsAPIResponse
{
    [JsonPropertyName("object")]
    public string Object { get; set; }
    [JsonPropertyName("data")]
    public List<ResponseData> Data { get; set; }
    [JsonPropertyName("model")]
    public string Model { get; set; }
    [JsonPropertyName("usage")]
    public TokenUsage Usage { get; set; }
}

public class ResponseData
{
    [JsonPropertyName("object")]
    public string Object { get; set; }
    [JsonPropertyName("index")]
    public int Index { get; set; }
    [JsonPropertyName("embedding")]
    public float[] Embedding { get; set; }
}

public class TokenUsage
{
    [JsonPropertyName("prompt_tokens")]
    public int PromptTokens { get; set; }
    [JsonPropertyName("total_tokens")]
    public int TotalTokens { get; set; }
}

