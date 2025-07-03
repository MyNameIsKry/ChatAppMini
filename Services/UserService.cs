using ChatAppMini.Models;
using ChatAppMini.Data;
using Microsoft.EntityFrameworkCore;
using ChatAppMini.DTOs.User;

namespace ChatAppMini.Services;

public interface IUserService
{
    Task<List<ResponseUserDto>> GetUsersAsync();
    Task<ResponseUserDto> CreateUserAsync(RequestUserDto user);
    Task<ResponseUserDto?> GetUsersByIdAsync(Guid id);
}

public class UserService : IUserService
{
    private readonly IUserRepository _repo;

    public UserService(IUserRepository repo)
    {
        _repo = repo;
    }

    public async Task<List<ResponseUserDto>> GetUsersAsync()
    {
        List<ResponseUserDto> users = await _repo.GetUsersAsync();

        return users;
    }

    public async Task<ResponseUserDto> CreateUserAsync(RequestUserDto user)
    {
        await _repo.CreateUserAsync(user);
        await _repo.SaveChangesAsync();

        ResponseUserDto responseUser = new ResponseUserDto
        {
            Name = user.Name,
            Email = user.Email
        };

        return responseUser;
    }

    public async Task<ResponseUserDto?> GetUsersByIdAsync(Guid id)
    {
        ResponseUserDto? user = await _repo.GetUsersByIdAsync(id);

        return user;
    }
}