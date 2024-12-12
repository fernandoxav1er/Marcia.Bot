using Marcia.Bot.Domain.Interfaces;
using Marcia.Bot.Domain.Models;
using MongoDB.Driver;

namespace Marcia.Bot.Infrastructure.MongoDB.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IMongoCollection<UserRequest> _collection;

    public UserRepository(IMongoDatabase database)
    {
        _collection = database.GetCollection<UserRequest>("UserRequests");
    }

    public async Task AddAsync(UserRequest userRequest)
    {
        await _collection.InsertOneAsync(userRequest);
    }

    public async Task<UserRequest> GetByIdAsync(string id)
    {
        return await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();
    }
}
