namespace ChatAppMini.DTOs.Conversation;
public class ResponseConversationDTO
{
    public Guid Id { get; set; }

    public List<Message> Messages { get; set; } = null!;
    public List<ConversationUser> Participants { get; set; } = null!;
}