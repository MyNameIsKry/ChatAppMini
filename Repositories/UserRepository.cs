using ChatAppMini.Models;
using ChatAppMini.Data;
using Microsoft.EntityFrameworkCore;
using ChatAppMini.DTOs.User;

public interface IUserRepository
{
    Task<List<ResponseUserDto>> GetUsersAsync();

    Task CreateUserAsync(RequestUserDto user);

    Task<ResponseUserDto?> GetUsersByIdAsync(Guid id);
    
    Task SaveChangesAsync();
}

class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<ResponseUserDto>> GetUsersAsync() =>
        await _context.Users
            .Select(u => new ResponseUserDto
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email
                // Add other properties as needed
            })
            .ToListAsync();

    public async Task CreateUserAsync(RequestUserDto user)
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

    public async Task<ResponseUserDto?> GetUsersByIdAsync(Guid id) =>
        await _context.Users
            .Where(u => u.Id == id)
            .Select(u => new ResponseUserDto
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                CreatedAt = u.CreatedAt,
                UpdatedAt = u.UpdatedAt
            })
            .FirstOrDefaultAsync();
}