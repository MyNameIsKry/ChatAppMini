namespace ChatAppMini.DTOs.Message;

public class RequestMessageDto
{
    public string Content { get; set; } = string.Empty;
    public Guid ConversationId { get; set; }
}

public class ResponseMessageDto
{
    public Guid Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime SentAt { get; set; }
    public Guid SenderId { get; set; }
    public string SenderName { get; set; } = string.Empty;
    public Guid ConversationId { get; set; }
}
