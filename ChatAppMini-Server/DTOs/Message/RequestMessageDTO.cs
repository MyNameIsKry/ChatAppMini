using System.ComponentModel.DataAnnotations;

public class RequestMessageDTO
{
    [Required]
    public string Content { get; set; } = null!;
    public DateTime SentAt { get; set; }    
}