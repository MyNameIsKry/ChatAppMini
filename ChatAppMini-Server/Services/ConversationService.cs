using Utils;
using ChatAppMini.DTOs.Conversation;

namespace ChatAppMini.Services.Conversations;

public interface IConversationService
{
    Task<ServiceResult<ResponseConversationDTO?>> GetConversationAsync(Guid id);
    Task<ServiceResult<Conversation>> CreateConversationAsync(Guid userId);
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

    public async Task<ServiceResult<Conversation>> CreateConversationAsync(Guid userId)
    {
        try
        {
            var conversation = await _repo.CreateConversationAsync(userId);
            await _repo.SaveChangesAsync();
            return ServiceResult<Conversation>.Success(conversation);
        }
        catch (Exception ex)
        {
            Logger.LogError("Error while creating conversation", ex);
            return ServiceResult<Conversation>.Fail("Error while creating conversation");
        }
    }
}