using System.ComponentModel.DataAnnotations;
using ChatAppMini.Models;
using Microsoft.EntityFrameworkCore;

public class ConversationUser
{
    public Guid ConversationId { get; set; }
    public Conversation Conversation { get; set; } = null!;

    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
}