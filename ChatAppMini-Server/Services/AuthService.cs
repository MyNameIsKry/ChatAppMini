using Utils;
using ChatAppMini.DTOs.Auth;
using Azure;

public interface IAuthService
{
    Task<ServiceResult<ResponseLoginDto>> LoginAsync(RequestLoginDto user);
}

public class AuthService : IAuthService
{
    private readonly IUserRepository _repo;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthService(IUserRepository repo, IHttpContextAccessor httpContextAccessor)
    {
        _repo = repo;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ServiceResult<ResponseLoginDto>> LoginAsync(RequestLoginDto user)
    {
        if (user == null || string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password))
        {
            return ServiceResult<ResponseLoginDto>.Fail("Invalid login data provided.");
        }

        User? existingUser = await _repo.GetUsersByEmailAsync(user.Email);
        if (existingUser == null)
        {
            return ServiceResult<ResponseLoginDto>.Fail("Email or password is incorrect.");
        }

        bool isPasswordValid = HashPasswordUtil.VerifyPassword(user.Password, existingUser.Password);
        if (!isPasswordValid)
        {
            return ServiceResult<ResponseLoginDto>.Fail("Email or password is incorrect.");
        }

        var jwtSettings = ProgramGlobals.JwtSettingsInstance;
        string accessToken = GenerateTokenUtil.GenerateAccessToken(existingUser.Id.ToString(), existingUser.Email, existingUser.Name, jwtSettings);
        string refreshToken = GenerateTokenUtil.GenerateRefreshToken();

        ResponseLoginDto responseLogin = new ResponseLoginDto
        {
            Id = existingUser.Id,
            Name = existingUser.Name,
            Email = existingUser.Email,
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            CreatedAt = existingUser.CreatedAt,
            UpdatedAt = existingUser.UpdatedAt
        };

        var CookiesOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTime.UtcNow.AddDays(7),
            SameSite = SameSiteMode.None,
            Secure = true
        };

        try
        {
            _httpContextAccessor.HttpContext?.Response.Cookies.Append("accessToken", accessToken, CookiesOptions);
            _httpContextAccessor.HttpContext?.Response.Cookies.Append("refreshToken", refreshToken, CookiesOptions);
        }
        catch (Exception ex)
        {
            return ServiceResult<ResponseLoginDto>.Fail($"An error occurred while setting cookies: {ex.Message}");
        }

        return ServiceResult<ResponseLoginDto>.Success(responseLogin);
    }
}