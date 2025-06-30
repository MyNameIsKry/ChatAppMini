using ChatAppMini.Models;
using ChatAppMini.Data;
using Microsoft.EntityFrameworkCore;

public interface IUserService
{
    Task<ApiResponse<List<User>>> GetUsersAsync();
    Task<ApiResponse<CreateUserDto>> CreateUserAsync(CreateUserDto user);
}

public class UserService : IUserService
{
    private readonly IUserRepository _repo;

    public UserService(IUserRepository repo)
    {
        _repo = repo;
    }

    public async Task<ApiResponse<List<User>>> GetUsersAsync()
    {
        List<User> users = await _repo.GetUsersAsync();

        return new ApiResponse<List<User>>(200, "Fetched users", users);
    }

    public async Task<ApiResponse<CreateUserDto>> CreateUserAsync(CreateUserDto user)
    {
        await _repo.CreateUserAsync(user);
        await _repo.SaveChangesAsync();

        return new ApiResponse<CreateUserDto>(201, "User created", user);
    }
}