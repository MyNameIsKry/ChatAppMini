namespace ChatAppMini.DTOs.Conversation;
public class RequestConversationDTO
{
    public Guid Id { get; set; }
    public List<ConversationUserDTO> Participants { get; set; } = null!;
}