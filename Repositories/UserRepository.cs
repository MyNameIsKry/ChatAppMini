using ChatAppMini.Models;
using ChatAppMini.Data;
using Microsoft.EntityFrameworkCore;

public interface IUserRepository
{
    Task<List<User>> GetUsersAsync();
    Task CreateUserAsync(User user);
    
    Task SaveChangesAsync();
}

class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<User>> GetUsersAsync() => await _context.Users.ToListAsync();

    public async Task CreateUserAsync(User user) => await _context.Users.AddAsync(user);

    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
}