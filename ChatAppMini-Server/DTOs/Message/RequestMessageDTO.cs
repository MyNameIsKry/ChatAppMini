using System.ComponentModel.DataAnnotations;

public class RequestMessageDTO
{
    [Required]
    public Guid SenderId { get; set; }

    public Guid? ReceiverId { get; set; }

    public string? GroupId { get; set; }

    [Required]
    public string Content { get; set; } = null!;
    public DateTime SentAt { get; set; }    
}