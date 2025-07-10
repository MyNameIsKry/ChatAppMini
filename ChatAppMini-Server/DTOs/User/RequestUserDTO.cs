using System.ComponentModel.DataAnnotations;

namespace ChatAppMini.DTOs.User;

public class RequestUserDto
{
    [Required]
    [StringLength(30, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 30 characters.")]
    public string Name { get; set; } = null!;

    [Required]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public string Email { get; set; } = null!;

    [Required]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long.")]
    public string Password { get; set; } = null!;
    [Required]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Confirm Password must be at least 6 characters long.")]
    public string ConfirmPassword { get; set; } = null!;
}
