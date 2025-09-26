using Microsoft.AspNetCore.Mvc;
using ChatAppMini.Services;
using ChatAppMini.DTOs.User;
using Utils;
using ChatAppMini.DTOs.Auth;

namespace ChatAppMini.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IAuthService _authService;

    public AuthController(
        IUserService userService,
        IAuthService authService
    )
    {
        _userService = userService;
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<ApiResponse<ResponseUserDto>> Register(RequestUserDto userDto)
    {
        try
        {
            ServiceResult<ResponseUserDto> createdUser = await _userService.CreateUserAsync(userDto);

            if (!createdUser.IsSuccess)
            {
                return new ApiResponse<ResponseUserDto>(400, createdUser.Message, null);
            }

            return new ApiResponse<ResponseUserDto>(201, "User registered successfully.", createdUser.Data);
        }
        catch (Exception ex)
        {
            Logger.LogError("An error occurred while registering the user.", ex);
            return new ApiResponse<ResponseUserDto>(status: 500, "An error occurred while registering the user.", null);
        }
    }

    [HttpPost("login")]
    public async Task<ApiResponse<ResponseLoginDto>> Login(RequestLoginDto loginDto)
    {
        try
        {
            ServiceResult<ResponseLoginDto> loginResult = await _authService.LoginAsync(loginDto);

            if (!loginResult.IsSuccess)
            {
                return new ApiResponse<ResponseLoginDto>(400, loginResult.Message, null);
            }

            return new ApiResponse<ResponseLoginDto>(200, "User logged in successfully.", loginResult.Data);
        }
        catch (Exception ex)
        {
            Logger.LogError("An error occurred while logging in.", ex);
            return new ApiResponse<ResponseLoginDto>(500, "An error occurred while logging in.", null);
        }
    }

    [HttpPost("logout")]
    public ApiResponse<string> Logout()
    {
        try
        {
            ServiceResult<string> logoutResult = _authService.Logout();

            if (!logoutResult.IsSuccess)
            {
                return new ApiResponse<string>(400, logoutResult.Message, null);
            }

            return new ApiResponse<string>(200, "User logged out successfully.", logoutResult.Data);
        }
        catch (Exception ex)
        {
            Logger.LogError("An error occurred while logging out.", ex);
            return new ApiResponse<string>(500, "An error occurred while logging out.", null);
        }
    }
}