using Marcia.Bot.Domain.Extensions;
using Marcia.Bot.Domain.Models;
using Newtonsoft.Json.Linq;

namespace Marcia.Bot.Infrastructure.HttpClients;

public interface IBrasilApiHttpClient
{
    Task<BaseResponse<EnderecoBrasilApiResponse>> ObterEnderecoPorCep(string cep);
}

public class BrasilApiHttpClient : IBrasilApiHttpClient
{
    private readonly HttpClient _httpClient;

    public BrasilApiHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<BaseResponse<EnderecoBrasilApiResponse>> ObterEnderecoPorCep(string cep)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"https://brasilapi.com.br/api/cep/v1/{cep}");
        var response = await _httpClient.SendAsync(request);
        var content = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            var objResponse = System.Text.Json.JsonSerializer.Deserialize<EnderecoBrasilApiResponse>(content);
            return objResponse.ToBaseResponse(response.StatusCode);
        }

        return new BaseResponse<EnderecoBrasilApiResponse>
        {
            CodigoHttp = response.StatusCode,
            DadosRetorno = null,
            ErroRetorno = JObject.Parse(content)["message"].ToString()
        };
    }
}
