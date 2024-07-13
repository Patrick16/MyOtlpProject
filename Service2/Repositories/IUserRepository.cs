using Models;
using Service2.Models;

namespace Service2.Repositories;

public interface IUserRepository
{
    Task<User> CreateUserAsync(UserCreateRequest request);
    Task<User> GetUserByIdAsync(long id);
}