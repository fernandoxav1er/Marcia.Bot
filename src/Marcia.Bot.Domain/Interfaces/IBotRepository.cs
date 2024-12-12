using Marcia.Bot.Domain.Models;

namespace Marcia.Bot.Domain.Interfaces;

public interface IBotRepository
{
    Task AddAsync(BotResponse botResponse);
    Task<BotResponse> GetByIdAsync(string id);
}
