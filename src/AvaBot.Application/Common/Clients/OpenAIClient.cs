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

        var serializedContent = JsonSerializer.Serialize(content);
        var request = new HttpRequestMessage(HttpMethod.Post, _openAPISettings.EmbeddingsBaseUrl)
        {
            Headers = {
                Authorization = new AuthenticationHeaderValue("Bearer", _openAPISettings.APIKey)
            },
            Content = new StringContent(serializedContent, Encoding.UTF8, "application/json")
        };

        var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseContentRead);

        var responseAsString = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<EmbeddingsAPIResponse>(responseAsString);
    }

    public async Task<ChatCompletionAPIResponse> CreateChatCompletion(List<Message> messages)
    {
        var content = new ChatCompletionAPIRequest
        {
            Model = _openAPISettings.ChatModel!,
            Messages = messages
        };

        var serializedContent = JsonSerializer.Serialize(content);

        var request = new HttpRequestMessage(HttpMethod.Post, _openAPISettings.ChatBaseUrl)
        {
            Headers = {
                Authorization = new AuthenticationHeaderValue("Bearer", _openAPISettings.APIKey)
            },
            Content = new StringContent(serializedContent, Encoding.UTF8, "application/json")
        };

        var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseContentRead);

        var responseAsString = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<ChatCompletionAPIResponse>(responseAsString);
    }
}

#region Embeddings Models
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
    public EmbeddingsTokenUsage Usage { get; set; }
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

public class EmbeddingsTokenUsage
{
    [JsonPropertyName("prompt_tokens")]
    public int PromptTokens { get; set; }
    [JsonPropertyName("total_tokens")]
    public int TotalTokens { get; set; }
}
#endregion

#region Chat completion Models

public class ChatCompletionAPIRequest
{
    [JsonPropertyName("model")]
    public string Model { get; set; }
    [JsonPropertyName("messages")]
    public List<Message> Messages { get; set; }
}

public class Message
{
    [JsonPropertyName("role")]
    public string Role { get; set; }
    [JsonPropertyName("content")]
    public string Content { get; set; }
}

public class ChatCompletionAPIResponse
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
    [JsonPropertyName("object")]
    public string Object { get; set; }
    [JsonPropertyName("created")]
    public int Created { get; set; }
    [JsonPropertyName("model")]
    public string Model { get; set; }
    [JsonPropertyName("choices")]
    public List<Choice> Choices { get; set; }
    [JsonPropertyName("usage")]
    public ChatTokenUsage Usage { get; set; }
}

public class ChatTokenUsage
{
    [JsonPropertyName("prompt_tokens")]
    public int PromptTokens { get; set; }
    [JsonPropertyName("completion_tokens")]
    public int CompletionTokens { get; set; }
    [JsonPropertyName("total_tokens")]
    public int TotalTokens { get; set; }
}

public class Choice
{
    [JsonPropertyName("index")]
    public int Index { get; set; }
    [JsonPropertyName("message")]
    public Message Message { get; set; }
    [JsonPropertyName("finish_reason")]
    public string FinishReason { get; set; }
}

#endregion


