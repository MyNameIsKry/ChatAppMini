using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ChatAppMini.Data;
using ChatAppMini.Models;
using ChatAppMini.Services;
using Utils;
using ChatAppMini.DTOs.User;

namespace ChatAppMini.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    public UsersController(IUserService userService) => _userService = userService;

    [HttpGet]
    public async Task<ApiResponse<List<ResponseUserDto>>> GetUsers()
    {
        var result = await _userService.GetUsersAsync();

        try
        {
            return new ApiResponse<List<ResponseUserDto>>(200, "Fetched users", result);
        }
        catch (Exception ex)
        {
            Logger.LogError($"Error fetching users: {ex.Message}", ex);
            return new ApiResponse<List<ResponseUserDto>>(500, "An error occurred while fetching users", null);
        }
    }

    [HttpGet("{id}")]
    public async Task<ApiResponse<ResponseUserDto>> GetUserById(Guid id)
    {
        if (string.IsNullOrEmpty(id.ToString()))
        {
            return new ApiResponse<ResponseUserDto>(400, "Invalid user ID", null);
        }

        try
        {
            var result = await _userService.GetUsersByIdAsync(id);
            if (result == null)
            {
                return new ApiResponse<ResponseUserDto>(404, "User not found", null);
            }
            return new ApiResponse<ResponseUserDto>(200, "User found", result);
        }
        catch (Exception ex)
        {
            Logger.LogError($"Error fetching user with ID {id}: {ex.Message}", ex);
            return new ApiResponse<ResponseUserDto>(500, $"An error occurred while fetching the user", null);
        }
    }
}
