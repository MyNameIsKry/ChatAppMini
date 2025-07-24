using Utils;
using ChatAppMini.DTOs.Conversation;

namespace ChatAppMini.Services.Conversations;

public interface IConversationService
{
    Task<ServiceResult<ResponseConversationDTO?>> GetConversationAsync(Guid id);
    Task<ServiceResult<ResponseConversationDTO>> CreateConversationAsync(RequestConversationDTO requestConversation);
}

public class ConversationService : IConversationService
{
    private readonly IConversationRepository _repo;

    public ConversationService(IConversationRepository repo) => _repo = repo;

    public async Task<ServiceResult<ResponseConversationDTO?>> GetConversationAsync(Guid id)
    {
        try
        {   
            if (!await _repo.IsConversationExistsAsync(id))
            {
                return ServiceResult<ResponseConversationDTO?>.Fail("Conversation not found.");
            }
            
            return ServiceResult<ResponseConversationDTO?>.Success(await _repo.GetConversationAsync(id));
        }
        catch (Exception ex)
        {
            Logger.LogError("Error while fetching conversation", ex);
            return ServiceResult<ResponseConversationDTO?>.Fail("Error while fetching conversation");
        }
    }

    public async Task<ServiceResult<ResponseConversationDTO>> CreateConversationAsync(RequestConversationDTO requestConversation)
    {
        try
        {
            var conversation = await _repo.CreateConversationAsync(requestConversation);
            await _repo.SaveChangesAsync();
            return ServiceResult<ResponseConversationDTO>.Success(conversation);
        }
        catch (Exception ex)
        {
            Logger.LogError("Error while creating conversation", ex);
            return ServiceResult<ResponseConversationDTO>.Fail("Error while creating conversation");
        }
    }
}