using Utils;
using ChatAppMini.DTOs.Conversation;

namespace ChatAppMini.Services.Conversations;
public interface IConversationService
{
    Task<ServiceResult<ResponseConversationDTO?>> GetConversationAsync(Guid id);
}

public class ConversationService : IConversationService
{
    private readonly IConversationRepository _repo;

    public ConversationService(IConversationRepository repo) => _repo = repo;

    public async Task<ServiceResult<ResponseConversationDTO?>> GetConversationAsync(Guid id)
    {
        try
        {
            return ServiceResult<ResponseConversationDTO?>.Success(await _repo.GetConversationAsync(id));
        }
        catch (Exception ex)
        {
            Logger.LogError("Error while fetching conversation", ex);
            return ServiceResult<ResponseConversationDTO?>.Fail("Error while fetching conversation");
        }
    }
}