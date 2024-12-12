using Discord;
using Discord.Commands;
using Marcia.Bot.Domain.Extensions;
using Marcia.Bot.Domain.Interfaces;
using System.Text;

namespace Marcia.Bot.Application.Commands;

public class HelpCommand : ModuleBase<SocketCommandContext>, IGetMessageReference
{
    private readonly CommandService _commandService;
    public MessageReference MessageReference => new MessageReference(Context.Message.Id, Context.Channel.Id, Context.Guild.Id);

    public HelpCommand(CommandService commandService)
    {
        _commandService = commandService;
    }

    [Command("help", RunMode = RunMode.Async)]
    [Summary("Exibe uma lista de todos os comandos com suas descrições.")]
    public async Task ExecuteAsync()
    {
        var modules = _commandService.Modules;
        var sb = new StringBuilder();

        var number = 1;
        foreach (var module in modules)
        {
            foreach (var command in module.Commands)
            {
                var description = command.Summary ?? "Sem descrição";
                sb.AppendLine($"{number}º {command.Name} - {description}");
                number++;
            }
        }

        var embed = new EmbedBuilder()
            .WithTitle("Lista de Comandos")
            .WithDescription(sb.ToString())
            .WithColor(Color.Green)
            .WithFooter(footer => footer.Text = "Use !comando para mais detalhes")
            .WithTimestamp(DateTimeOffset.UtcNow)
            .Build();

        await ReplyAsync(embed: embed, messageReference: MessageReference);
    }
}