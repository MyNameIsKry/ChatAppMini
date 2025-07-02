using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ChatAppMini.Data;
using ChatAppMini.Models;
using ChatAppMini.Services;

namespace ChatAppMini.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    public UsersController(IUserService userService) => _userService = userService;

    [HttpGet]
    public async Task<ApiResponse<List<User>>> GetUsers()
    {
        var result = await _userService.GetUsersAsync();

        try
        {
            return new ApiResponse<List<User>>(200, "Fetched users", result);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new ApiResponse<List<User>>(500, "An error occurred while fetching users", null);
        }
    }

    [HttpGet("{id}")]
    public async Task<ApiResponse<User>> GetUserById(Guid id)
    {
        if (string.IsNullOrEmpty(id.ToString()))
        {
            return new ApiResponse<User>(400, "Invalid user ID", null);
        }

        try
        {
            var result = await _userService.GetUsersByIdAsync(id);
            if (result == null)
            {
                return new ApiResponse<User>(404, "User not found", null);
            }
            return new ApiResponse<User>(200, "User found", result);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new ApiResponse<User>(500, $"An error occurred while fetching the user", null);
        }
    }
}
