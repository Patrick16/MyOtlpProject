using Models;
using PostgresDb;
using Service2.Models;

namespace Service2.Repositories;

public class UserRepository : IUserRepository
{
    private readonly MyContext _context;

    public UserRepository(MyContext context)
    {
        _context = context;
    }

    public async Task<User> CreateUserAsync(UserCreateRequest request)
    {
        var user = new User
        {
            FirstName = request.FirstName,
            LastName = request.LastName
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return user;
    }

    public async Task<User> GetUserByIdAsync(long id)
    {
        var res = await _context.Users.FindAsync(id);
        return res ?? throw new Exception("User not found");
    }
}
