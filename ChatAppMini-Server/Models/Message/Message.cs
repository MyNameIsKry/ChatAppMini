using System.ComponentModel.DataAnnotations;

public class Message
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Content { get; set; } = string.Empty;
    public DateTime SentAt { get; set; }

    public Guid SenderId { get; set; }
    public User Sender { get; set; } = null!;

    public Guid ConversationId { get; set; }
    public Conversation Conversation { get; set; } = null!;
}
