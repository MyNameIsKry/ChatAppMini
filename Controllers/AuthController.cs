using Microsoft.AspNetCore.Mvc;
using ChatAppMini.Services;

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
    public async Task<ApiResponse<CreateUserDto>> Register(CreateUserDto userDto)
    {
        if (userDto == null || string.IsNullOrEmpty(userDto.Name) || string.IsNullOrEmpty(userDto.Email) || string.IsNullOrEmpty(userDto.Password))
        {
            return new ApiResponse<CreateUserDto>(400, "Invalid user data provided.", null);
        }

        try
        {
            var createdUser = await _userService.CreateUserAsync(userDto);
            return new ApiResponse<CreateUserDto>(201, "User registered successfully.", createdUser);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new ApiResponse<CreateUserDto>(500, "An error occurred while registering the user.", null);
        }
    }
}