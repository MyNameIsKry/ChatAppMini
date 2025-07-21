namespace ChatAppMini.DTOs.Conversation;
public class ResponseConversationDTO
{
    public Guid Id { get; set; }

    public List<ResponseMessageDTO> Messages { get; set; } = null!;
    public List<ResponseParticipantDTO> Participants { get; set; } = null!;
}