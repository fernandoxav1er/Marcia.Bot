using Discord;
using Discord.Commands;
using Marcia.Bot.Domain.Interfaces;

namespace Marcia.Bot.Application.Commands;

public class PingCommand : ModuleBase<SocketCommandContext>, IGetMessageReference
{
    public MessageReference MessageReference => new MessageReference(Context.Message.Id, Context.Channel.Id, Context.Guild.Id);

    [Command("ping", RunMode = RunMode.Async)]
    [Summary("Verificar a conectividade e o funcionamento básico do bot.")]
    public async Task ExecuteAsync()
    {
        await ReplyAsync("Pong!", messageReference: MessageReference);
    }
}
