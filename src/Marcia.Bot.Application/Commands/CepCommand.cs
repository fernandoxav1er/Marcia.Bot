using Discord;
using Discord.Commands;
using Marcia.Bot.Domain.Extensions;
using Marcia.Bot.Domain.Interfaces;
using Marcia.Bot.Infrastructure.HttpClients;

namespace Marcia.Bot.Application.Commands;

public class CepCommand : ModuleBase<SocketCommandContext>, IGetMessageReference
{
    private readonly IBrasilApiHttpClient _brasilApiService;
    public MessageReference MessageReference => new MessageReference(Context.Message.Id, Context.Channel.Id, Context.Guild.Id);

    public CepCommand(IBrasilApiHttpClient brasilApiService)
    {
        _brasilApiService = brasilApiService;
    }

    [Command("cep")]
    [Summary("Pesquisa um endereço com base no código postal (CEP) fornecido.")]
    public async Task ExecuteAsync(string cep)
    {
        try
        {
            var response = await _brasilApiService.ObterEnderecoPorCep(cep);

            if (response.CodigoHttp == System.Net.HttpStatusCode.OK && response.DadosRetorno != null)
            {
                await ReplyAsync(new FormatStringExtensions().FormatEndereco(cep, response.DadosRetorno), messageReference: MessageReference);
                return;
            }

            await ReplyAsync($"Não foi possível encontrar um endereço para o CEP {cep}. Erro: {response.ErroRetorno}", messageReference: MessageReference);
        }
        catch (Exception ex)
        {
            await ReplyAsync($"Ocorreu um erro: {ex.Message}");
        }
    }
}
