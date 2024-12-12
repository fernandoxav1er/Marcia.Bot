using Marcia.Bot.Domain.Models;

namespace Marcia.Bot.Domain.Extensions;

public class FormatStringExtensions
{
    public string FormatEndereco(string cep, EnderecoBrasilApiResponse endereco)
    {
        return $"```json\n{{\n\t\"CEP\": \"{cep}\",\n\t\"Estado\": \"{endereco.Estado}\",\n\t\"Cidade\": \"{endereco.Cidade}\",\n\t\"Região\": \"{endereco.Regiao}\",\n\t\"Rua\": \"{endereco.Rua}\",\n\t\"Service\": \"{endereco.Servico}\"\n}}```";
    }
}
