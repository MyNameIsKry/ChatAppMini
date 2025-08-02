using Utils;
using ChatAppMini.DTOs.Conversation;
using System.Security.Claims;

namespace ChatAppMini.Services.Conversations;

public interface IConversationService
{
    Task<ServiceResult<ResponseConversationDTO?>> GetConversationAsync(Guid id);
    Task<ServiceResult<ResponseConversationDTO>> CreateConversationAsync(RequestConversationDTO requestConversation);
    Task<ServiceResult<List<ResponseConversationDTO>>> GetUserConversationsAsync();
    Task<ServiceResult<bool>> AddParticipantAsync(Guid conversationId, Guid participantId);
    Task<ServiceResult<bool>> RemoveParticipantAsync(Guid conversationId, Guid participantId);
    Task<ServiceResult<bool>> DeleteConversationAsync(Guid conversationId);
}

public class ConversationService : IConversationService
{
    private readonly IConversationRepository _repo;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ConversationService(
        IConversationRepository repo,
        IHttpContextAccessor httpContextAccessor)
    {
        _repo = repo;
        _httpContextAccessor = httpContextAccessor;
    }

    private Guid? GetCurrentUserId()
    {
        var id = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return id != null ? Guid.Parse(id) : null;
    }

    public async Task<ServiceResult<ResponseConversationDTO?>> GetConversationAsync(Guid id)
    {
        try
        {   
            var userId = GetCurrentUserId();
            if (!userId.HasValue)
            {
                return ServiceResult<ResponseConversationDTO?>.Fail("User is not authenticated.");
            }

            if (!await _repo.IsConversationExistsAsync(id))
            {
                return ServiceResult<ResponseConversationDTO?>.Fail("Conversation not found.");
            }
            
            var conversation = await _repo.GetConversationAsync(id);
            if (conversation == null || !conversation.Participants.Any(p => p.UserId == userId))
            {
                return ServiceResult<ResponseConversationDTO?>.Fail("You don't have access to this conversation.");
            }
            
            return ServiceResult<ResponseConversationDTO?>.Success(conversation);
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
            var userId = GetCurrentUserId();
            if (!userId.HasValue)
            {
                return ServiceResult<ResponseConversationDTO>.Fail("User is not authenticated.");
            }

            // Add current user to participants if not already included
            if (!requestConversation.Participants.Any(p => p.UserId == userId))
            {
                requestConversation.Participants.Add(new ConversationUserDTO { UserId = userId.Value });
            }

            var conversation = await _repo.CreateConversationAsync(requestConversation);
            return ServiceResult<ResponseConversationDTO>.Success(conversation);
        }
        catch (Exception ex)
        {
            Logger.LogError("Error while creating conversation", ex);
            return ServiceResult<ResponseConversationDTO>.Fail("Error while creating conversation");
        }
    }

    public async Task<ServiceResult<List<ResponseConversationDTO>>> GetUserConversationsAsync()
    {
        try
        {
            var userId = GetCurrentUserId();
            if (!userId.HasValue)
            {
                return ServiceResult<List<ResponseConversationDTO>>.Fail("User is not authenticated.");
            }

            var conversations = await _repo.GetUserConversationsAsync(userId.Value);
            return ServiceResult<List<ResponseConversationDTO>>.Success(conversations);
        }
        catch (Exception ex)
        {
            Logger.LogError("Error while fetching user conversations", ex);
            return ServiceResult<List<ResponseConversationDTO>>.Fail("Error while fetching conversations");
        }
    }

    public async Task<ServiceResult<bool>> AddParticipantAsync(Guid conversationId, Guid participantId)
    {
        try
        {
            var userId = GetCurrentUserId();
            if (!userId.HasValue)
            {
                return ServiceResult<bool>.Fail("User is not authenticated.");
            }

            var conversation = await _repo.GetConversationAsync(conversationId);
            if (conversation == null || !conversation.Participants.Any(p => p.UserId == userId))
            {
                return ServiceResult<bool>.Fail("You don't have access to this conversation.");
            }

            await _repo.AddParticipantAsync(conversationId, participantId);
            return ServiceResult<bool>.Success(true);
        }
        catch (Exception ex)
        {
            Logger.LogError("Error while adding participant", ex);
            return ServiceResult<bool>.Fail("Error while adding participant");
        }
    }

    public async Task<ServiceResult<bool>> RemoveParticipantAsync(Guid conversationId, Guid participantId)
    {
        try
        {
            var userId = GetCurrentUserId();
            if (!userId.HasValue)
            {
                return ServiceResult<bool>.Fail("User is not authenticated.");
            }

            var conversation = await _repo.GetConversationAsync(conversationId);
            if (conversation == null || !conversation.Participants.Any(p => p.UserId == userId))
            {
                return ServiceResult<bool>.Fail("You don't have access to this conversation.");
            }

            if (userId == participantId)
            {
                return ServiceResult<bool>.Fail("You cannot remove yourself from the conversation.");
            }

            await _repo.RemoveParticipantAsync(conversationId, participantId);
            return ServiceResult<bool>.Success(true);
        }
        catch (Exception ex)
        {
            Logger.LogError("Error while removing participant", ex);
            return ServiceResult<bool>.Fail("Error while removing participant");
        }
    }

    public async Task<ServiceResult<bool>> DeleteConversationAsync(Guid conversationId)
    {
        try
        {
            var userId = GetCurrentUserId();
            if (!userId.HasValue)
            {
                return ServiceResult<bool>.Fail("User is not authenticated.");
            }

            var conversation = await _repo.GetConversationAsync(conversationId);
            if (conversation == null || !conversation.Participants.Any(p => p.UserId == userId))
            {
                return ServiceResult<bool>.Fail("You don't have access to this conversation.");
            }

            await _repo.DeleteConversationAsync(conversationId);
            return ServiceResult<bool>.Success(true);
        }
        catch (Exception ex)
        {
            Logger.LogError("Error while deleting conversation", ex);
            return ServiceResult<bool>.Fail("Error while deleting conversation");
        }
    }
}