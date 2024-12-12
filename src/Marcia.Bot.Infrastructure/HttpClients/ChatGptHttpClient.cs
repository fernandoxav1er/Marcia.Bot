using Marcia.Bot.Domain.Models;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;

namespace Marcia.Bot.Infrastructure.HttpClients;

public interface IChatGptHttpClient
{
    Task<ChatResponse> SendChatRequestAsync(string endpoint, ChatRequest.Send request);
}
public class ChatGptHttpClient : IChatGptHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ChatGptHttpClient> _logger;
    
    private readonly string _baseUrl = "http://localhost:1337/v1/";

    public ChatGptHttpClient(HttpClient httpClient, ILogger<ChatGptHttpClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<ChatResponse> SendChatRequestAsync(string endpoint, ChatRequest.Send request)
    {
        try
        {
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, new Uri(new Uri(_baseUrl), endpoint))
            {
                Content = new StringContent(
                    JsonSerializer.Serialize(request),
                    Encoding.UTF8,
                    "application/json"
                )
            };

            using var response = await _httpClient.SendAsync(httpRequest);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Request falhou: {StatusCode} - {Content}", response.StatusCode, content);
                throw new HttpRequestException($"Request falhou com status {response.StatusCode}");
            }

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var chatResponse = JsonSerializer.Deserialize<ChatResponse>(content, options);

            if (chatResponse == null || chatResponse.Choices == null || chatResponse.Choices.Count == 0)
            {
                _logger.LogError("Resposta inválida ou sem dados: {Content}", content);
                throw new InvalidOperationException("A resposta não contém dados válidos.");
            }

            return chatResponse;
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Erro ao desserializar a resposta do endpoint {Endpoint}", endpoint);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao enviar requisição para o endpoint {Endpoint}", endpoint);
            throw;
        }
    }
}
