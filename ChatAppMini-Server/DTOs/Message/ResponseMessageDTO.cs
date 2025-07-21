public class ResponseMessageDTO
{
    public Guid Id { get; set; }
    public Guid SenderId { get; set; }
    public string Content { get; set; } = null!;
    public DateTime SentAt { get; set; }

}