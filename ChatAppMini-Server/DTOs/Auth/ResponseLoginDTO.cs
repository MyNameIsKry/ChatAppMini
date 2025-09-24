using System.ComponentModel.DataAnnotations;

namespace ChatAppMini.DTOs.Auth;
public class ResponseLoginDto
{
    public Guid Id { get; set; }
    
    public string Name { get; set; } = null!;
    
    public string Email { get; set; } = null!;

    public string AccessToken { get; set; } = null!;

    public string RefreshToken { get; set; } = null!;
    public string AvatarUrl { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}