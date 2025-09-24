using ChatAppMini.Data;
using Microsoft.EntityFrameworkCore;
using ChatAppMini.DTOs.User;

public interface IUserRepository
{
    Task<List<ResponseUserDto>> GetUsersAsync();
    Task<User> CreateUserAsync(RequestUserDto user);
    Task<ResponseUserDto?> GetUsersByIdAsync(Guid id);
    Task SaveChangesAsync();
    Task<bool> UserExistsAsync(string email);
    Task<User?> GetUsersByEmailAsync(string email);
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
                Email = u.Email,
                AvatarUrl = u.AvatarUrl,
                CreatedAt = u.CreatedAt,
                UpdatedAt = u.UpdatedAt
            })
            .ToListAsync();

    public async Task<User> CreateUserAsync(RequestUserDto user)
    {
        var newUser = new User
        {

            Name = user.Name,
            Email = user.Email,
            Password = user.Password,
        };
        await _context.Users.AddAsync(newUser);

        return newUser;
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
                AvatarUrl = u.AvatarUrl,
                CreatedAt = u.CreatedAt,
                UpdatedAt = u.UpdatedAt
            })
            .FirstOrDefaultAsync();

    public async Task<bool> UserExistsAsync(string email)
    {
        return await _context.Users.AnyAsync(u => u.Email == email);
    }

    public async Task<User?> GetUsersByEmailAsync(string email) =>
        await _context.Users
            .Where(u => u.Email == email)
            .Select(u => new User
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                Password = u.Password,
                AvatarUrl = u.AvatarUrl,
                CreatedAt = u.CreatedAt,
                UpdatedAt = u.UpdatedAt
            })
            .FirstOrDefaultAsync();
}