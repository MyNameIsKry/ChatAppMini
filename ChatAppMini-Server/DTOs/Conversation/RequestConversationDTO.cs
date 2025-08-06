namespace ChatAppMini.DTOs.Conversation;
public class RequestConversationDTO
{
    public List<UserDTO> Participants { get; set; } = null!;
}