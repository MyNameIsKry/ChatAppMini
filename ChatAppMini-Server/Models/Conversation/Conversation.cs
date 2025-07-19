using System.ComponentModel.DataAnnotations;
using ChatAppMini.Models;

public class Conversation
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    public List<Message> Messages { get; set; } = new();
    public List<ConversationUser> Participants { get; set; } = new();
}