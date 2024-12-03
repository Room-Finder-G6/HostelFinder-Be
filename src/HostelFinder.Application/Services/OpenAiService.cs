using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using HostelFinder.Application.DTOs.ChatAI.Request;
using HostelFinder.Application.DTOs.ChatAI.Response;
using HostelFinder.Application.Interfaces.IServices;
using Microsoft.Extensions.Configuration;

namespace HostelFinder.Application.Services;

public class OpenAiService : IOpenAiService
{
    private readonly IConfiguration _configuration;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly JsonSerializerOptions _jsonOptions;

    public OpenAiService(IConfiguration configuration, IHttpClientFactory httpClientFactory)
    {
        _configuration = configuration;
        _httpClientFactory = httpClientFactory;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }
    public async Task<OpenAiChatResponse> GenerateAsync(OpenAiChatRequest request)
    {
        if (request == null || request.Messages == null || request.Messages.Count == 0)
        {
            throw new ArgumentException("Messages are required.");
        }

        var apiKey = _configuration["OpenAI:ApiKey"];
        if (string.IsNullOrEmpty(apiKey))
        {
            throw new InvalidOperationException("OpenAI API Key is not configured.");
        }

        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

        var apiUrl = "https://api.openai.com/v1/chat/completions";

        var payload = new
        {
            model = request.Model, // "gpt-3.5-turbo" hoặc "gpt-4"
            messages = request.Messages,
            temperature = request.Temperature
        };

        var content = new StringContent(JsonSerializer.Serialize(payload, _jsonOptions), Encoding.UTF8, "application/json");

        var response = await client.PostAsync(apiUrl, content);
        var responseString = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Error calling OpenAI API: {responseString}");
        }

        var openAiResponse = JsonSerializer.Deserialize<OpenAiChatResponse>(responseString, _jsonOptions);
        return openAiResponse;
    }
}