using ChatAppMini.Models;
using ChatAppMini.Data;
using Microsoft.EntityFrameworkCore;

public interface IUserService
{
    Task<ApiResponse<List<User>>> GetUsersAsync();
    Task<ApiResponse<User>> CreateUserAsync(User user);
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

    public async Task<ApiResponse<User>> CreateUserAsync(User user)
    {
        await _repo.CreateUserAsync(user);
        await _repo.SaveChangesAsync();

        return new ApiResponse<User>(201, "User created", user);
    }
}