using Discord;

namespace Marcia.Bot.Domain.Interfaces;

public interface IGetMessageReference
{
    MessageReference MessageReference { get; }
}