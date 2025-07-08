using ChatAppMini.Models;
using ChatAppMini.DTOs.User;
using Utils;
using System.Security.Claims;

namespace ChatAppMini.Services;

public interface IUserService
{
    Task<ServiceResult<List<ResponseUserDto>>> GetUsersAsync();
    Task<ServiceResult<ResponseUserDto>> CreateUserAsync(RequestUserDto user);
    Task<ServiceResult<ResponseUserDto>> GetUsersByIdAsync(Guid id);
    Guid? UserId { get; }
    string? Username { get; }
    string? Email { get; }
    bool IsAuthenticated { get; }
}

public class UserService : IUserService
{
    private readonly IUserRepository _repo;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserService(
        IUserRepository repo,
        IHttpContextAccessor httpContextAccessor
    )
    {
        _repo = repo;
        _httpContextAccessor = httpContextAccessor;
    }

    private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

    public Guid? UserId
    {
        get
        {
            var id = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.TryParse(id, out var guid) ? guid : (Guid?)null;
        }
    }
    public string? Username => User?.FindFirst(ClaimTypes.Name)?.Value;
    public string? Email => User?.FindFirst(ClaimTypes.Email)?.Value;
    public bool IsAuthenticated => User?.Identity?.IsAuthenticated ?? false;

    public async Task<ServiceResult<List<ResponseUserDto>>> GetUsersAsync()
    {
        List<ResponseUserDto> users = await _repo.GetUsersAsync();

        return ServiceResult<List<ResponseUserDto>>.Success(users);
    }

    public async Task<ServiceResult<ResponseUserDto>> CreateUserAsync(RequestUserDto user)
    {
        if (user == null ||
            string.IsNullOrEmpty(user.Name) ||
            string.IsNullOrEmpty(user.Email) ||
            string.IsNullOrEmpty(user.Password) ||
            string.IsNullOrEmpty(user.ConfirmPassword))
        {
            return ServiceResult<ResponseUserDto>.Fail("Invalid user data provided.");
        }

        if (user.Password != user.ConfirmPassword)
        {
            return ServiceResult<ResponseUserDto>.Fail("Passwords do not match.");
        }

        if (await _repo.UserExistsAsync(user.Email))
        {
            return ServiceResult<ResponseUserDto>.Fail("User with this email already exists.");
        }

        string hashedPassword = HashPasswordUtil.HashPassword(user.Password);
        user.Password = hashedPassword;

        User newUser = await _repo.CreateUserAsync(user);
        await _repo.SaveChangesAsync();

        ResponseUserDto responseUser = new ResponseUserDto
        {
            Id = newUser.Id,
            Name = user.Name,
            Email = user.Email,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        return ServiceResult<ResponseUserDto>.Success(responseUser);
    }

    public async Task<ServiceResult<ResponseUserDto>> GetUsersByIdAsync(Guid id)
    {
        ResponseUserDto? user = await _repo.GetUsersByIdAsync(id);

        return user != null
            ? ServiceResult<ResponseUserDto>.Success(user)
            : ServiceResult<ResponseUserDto>.Fail("User not found.");
    }
}