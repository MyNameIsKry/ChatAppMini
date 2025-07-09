using Microsoft.AspNetCore.Mvc;
using ChatAppMini.Services;
using Utils;
using ChatAppMini.DTOs.User;
using Microsoft.AspNetCore.Authorization;

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
        ServiceResult<List<ResponseUserDto>> result = await _userService.GetUsersAsync();

        try
        {
            return new ApiResponse<List<ResponseUserDto>>(200, "Fetched users", result.Data);
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
        try
        {
            ServiceResult<ResponseUserDto> result = await _userService.GetUsersByIdAsync(id);

            if (!result.IsSuccess)
            {
                return new ApiResponse<ResponseUserDto>(404, result.Message, null);
            }

            return new ApiResponse<ResponseUserDto>(200, "User fetched successfully", result.Data);
        }
        catch (Exception ex)
        {
            Logger.LogError($"Error fetching user with ID {id}: {ex.Message}", ex);
            return new ApiResponse<ResponseUserDto>(500, $"An error occurred while fetching the user", null);
        }
    }

    [HttpGet("@me")]
    [Authorize]
    public async Task<ApiResponse<ResponseUserDto>> GetUserInfo()
    {
        try
        {
            if (!_userService.IsAuthenticated)
                return new ApiResponse<ResponseUserDto>(401, "Unauthorized", null);

            ServiceResult<ResponseUserDto> user = await _userService.GetUsersByIdAsync(_userService.UserId ?? Guid.Empty);

            return new ApiResponse<ResponseUserDto>(200, "User Info", user.Data);
        }
        catch (Exception ex)
        {
            Logger.LogError($"Error fetching user info {ex.Message}", ex);
            return new ApiResponse<ResponseUserDto>(500, $"An error occurred while fetching the user", null);
        }
    }
}
