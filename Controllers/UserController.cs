using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ChatAppMini.Data;
using ChatAppMini.Models;

namespace ChatAppMini.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    public UsersController(IUserService userService) => _userService = userService;

    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        var result = await _userService.GetUsersAsync();

        try
        {
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while fetching users.", error = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser(CreateUserDto user)
    {
        if (user == null)
        {
            return BadRequest(new { message = "User data is required." });
        }

        try
        {   
            var result = await _userService.CreateUserAsync(user);
            return CreatedAtAction(nameof(GetUsers), result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while creating the user.", error = ex.Message });
        }
    }
}
