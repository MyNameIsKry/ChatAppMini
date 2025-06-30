using ChatAppMini.Models;
using ChatAppMini.Data;
using Microsoft.EntityFrameworkCore;

public interface IUserService
{
    Task<ApiResponse<List<User>>> GetUsersAsync();
    Task<ApiResponse<CreateUserDto>> CreateUserAsync(CreateUserDto user);
    Task<ApiResponse<User?>> GetUsersByIdAsync(Guid id);
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

    public async Task<ApiResponse<User?>> GetUsersByIdAsync(Guid id)
    {
        User? user = await _repo.GetUsersByIdAsync(id);

        if (user == null)
        {
            return new ApiResponse<User?>(404, "User not found", null);
        }

        return new ApiResponse<User?>(200, "User found", user);
    }
}