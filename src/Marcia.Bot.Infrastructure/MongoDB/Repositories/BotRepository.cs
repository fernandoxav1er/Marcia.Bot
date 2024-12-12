using Marcia.Bot.Domain.Interfaces;
using Marcia.Bot.Domain.Models;
using MongoDB.Driver;

namespace Marcia.Bot.Infrastructure.MongoDB.Repositories;

public class BotRepository : IBotRepository
{
    private readonly IMongoCollection<BotResponse> _collection;

    public BotRepository(IMongoDatabase database)
    {
        _collection = database.GetCollection<BotResponse>("BotResponses");
    }

    public async Task AddAsync(BotResponse botResponse)
    {
        await _collection.InsertOneAsync(botResponse);
    }

    public async Task<BotResponse> GetByIdAsync(string id)
    {
        return await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();
    }
}
