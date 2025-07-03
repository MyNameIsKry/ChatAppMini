using ChatAppMini.Models;
using ChatAppMini.Data;
using Microsoft.EntityFrameworkCore;
using ChatAppMini.DTOs.User;

namespace ChatAppMini.Services;

public interface IUserService
{
    Task<List<User>> GetUsersAsync();
    Task<RequestUserDto> CreateUserAsync(RequestUserDto user);
    Task<User?> GetUsersByIdAsync(Guid id);
}

public class UserService : IUserService
{
    private readonly IUserRepository _repo;

    public UserService(IUserRepository repo)
    {
        _repo = repo;
    }

    public async Task<List<User>> GetUsersAsync()
    {
        List<User> users = await _repo.GetUsersAsync();

        return users;
    }

    public async Task<RequestUserDto> CreateUserAsync(RequestUserDto user)
    {
        await _repo.CreateUserAsync(user);
        await _repo.SaveChangesAsync();

        return user;
    }

    public async Task<User?> GetUsersByIdAsync(Guid id)
    {
        User? user = await _repo.GetUsersByIdAsync(id);

        return user;
    }
}