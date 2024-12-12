using Discord;
using Discord.Commands;

namespace Marcia.Bot.Application.Commands;

public class EmbedCommand : ModuleBase<SocketCommandContext>
{
    [Command("embed")]
    [Summary("Modelo de comando para enviar um embed.")]
    public async Task ExecuteAsync()
    {
        var embed = new EmbedBuilder()
            .WithTitle("Título do Embed")
            .WithDescription("Essa é a descrição do embed.")
            .WithColor(Color.Blue)
            .AddField("Campo 1", "Conteúdo do campo 1.", true)
            .AddField("Campo 2", "Conteúdo do campo 2.", true)
            .WithFooter(footer => footer.Text = "Rodapé do Embed")
            .WithTimestamp(DateTimeOffset.UtcNow)
            .Build();

        await ReplyAsync(embed: embed);
    }
}
