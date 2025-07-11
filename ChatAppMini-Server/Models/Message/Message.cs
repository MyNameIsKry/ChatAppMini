using System.ComponentModel.DataAnnotations;

public class Message {
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid SenderId { get; set; }

    public Guid? ReceiverId { get; set; }

    public string? GroupId { get; set; }

    public string Content { get; set; }
    public DateTime SentAt { get; set; }

    public bool IsGroupMessage => GroupId != null;
}
