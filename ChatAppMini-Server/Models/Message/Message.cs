public class Message {
    public int Id { get; set; }
    public string SenderId { get; set; }

    public string? ReceiverId { get; set; }

    public string? GroupId { get; set; }

    public string Content { get; set; }
    public DateTime SentAt { get; set; }

    public bool IsGroupMessage => GroupId != null;
}
