using ChatAppMini.Models;
using ChatAppMini.Data;
using Microsoft.EntityFrameworkCore;
using ChatAppMini.DTOs.User;

public interface IUserRepository
{
    Task<List<User>> GetUsersAsync();

    Task CreateUserAsync(CreateUserDto user);

    Task<User?> GetUsersByIdAsync(Guid id);
    
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

    public async Task CreateUserAsync(CreateUserDto user)
    {
        var newUser = new User
        {
            Name = user.Name,
            Email = user.Email,
            Password = user.Password,
        };
        await _context.Users.AddAsync(newUser);
    }

    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();

    public async Task<User?> GetUsersByIdAsync(Guid id) => await _context.Users.FindAsync(id);
}