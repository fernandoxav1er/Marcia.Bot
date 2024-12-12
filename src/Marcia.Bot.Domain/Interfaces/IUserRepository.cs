using Marcia.Bot.Domain.Models;

namespace Marcia.Bot.Domain.Interfaces;

public interface IUserRepository
{
    Task AddAsync(UserRequest userRequest);
    Task<UserRequest> GetByIdAsync(string id);
}
