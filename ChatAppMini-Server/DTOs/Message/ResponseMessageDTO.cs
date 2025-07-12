public class ResponseMessageDTO
{
    public Guid SenderId { get; set; }

    public Guid? ReceiverId { get; set; }

    public string? GroupId { get; set; }

    public string Content { get; set; } = null!;
    public DateTime SentAt { get; set; }    
}