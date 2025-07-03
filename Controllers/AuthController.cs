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
            Console.WriteLine(ex.Message);
            return new ApiResponse<ResponseUserDto>(500, "An error occurred while registering the user.", null);
        }
    }
}