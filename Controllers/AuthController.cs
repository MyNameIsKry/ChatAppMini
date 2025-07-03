using Microsoft.AspNetCore.Mvc;
using ChatAppMini.Services;
using ChatAppMini.DTOs.User;
using Utils;

namespace ChatAppMini.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;

    public AuthController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<ApiResponse<ResponseUserDto>> Register(RequestUserDto userDto)
    {
        if (userDto == null || string.IsNullOrEmpty(userDto.Name) || string.IsNullOrEmpty(userDto.Email) || string.IsNullOrEmpty(userDto.Password))
        {
            return new ApiResponse<ResponseUserDto>(400, "Invalid user data provided.", null);
        }

        try
        {
            var createdUser = await _userService.CreateUserAsync(userDto);
            return new ApiResponse<ResponseUserDto>(201, "User registered successfully.", createdUser);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new ApiResponse<ResponseUserDto>(500, "An error occurred while registering the user.", null);
        }
    }
}