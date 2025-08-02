using ChatAppMini.DTOs.User;
using Utils;
using System.Security.Claims;

namespace ChatAppMini.Services;

public interface IMessageService
{
    Task<ServiceResult<ResponseMessageDTO>> SendMessageAsync(RequestMessageDTO messageDto, Guid conversationId);
    Task<ServiceResult<List<ResponseMessageDTO>>> GetMessagesInConversationAsync(Guid conversationId, int take = 20, DateTime? before = null);
    Task<ServiceResult<ResponseMessageDTO>> UpdateMessageAsync(Guid messageId, string newContent);
    Task<ServiceResult<bool>> DeleteMessageAsync(Guid messageId);
}

public class MessageService : IMessageService
{
    private readonly IMessageRepository _repo;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConversationRepository _conversationRepo;
    private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

    public MessageService(
        IMessageRepository repo,
        IHttpContextAccessor httpContextAccessor,
        IConversationRepository conversationRepo
    )
    {
        _repo = repo;
        _httpContextAccessor = httpContextAccessor;
        _conversationRepo = conversationRepo;
    }

    private Guid? GetCurrentUserId()
    {
        var id = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return id != null ? Guid.Parse(id) : null;
    }

    private async Task<bool> IsUserInConversation(Guid conversationId, Guid userId)
    {
        var conversation = await _conversationRepo.GetConversationAsync(conversationId);
        return conversation?.Participants.Any(p => p.UserId == userId) ?? false;
    }

    public async Task<ServiceResult<ResponseMessageDTO>> SendMessageAsync(RequestMessageDTO messageDto, Guid conversationId)
    {
        if (User == null || User?.Identity?.IsAuthenticated != true)
        {
            return ServiceResult<ResponseMessageDTO>.Fail("User is not authenticated.");
        }

        var userId = GetCurrentUserId();
        if (!userId.HasValue)
        {
            return ServiceResult<ResponseMessageDTO>.Fail("Invalid user ID.");
        }

        if (!await IsUserInConversation(conversationId, userId.Value))
        {
            return ServiceResult<ResponseMessageDTO>.Fail("User is not part of this conversation.");
        }

        messageDto.SentAt = DateTime.UtcNow;
        var message = await _repo.SendMessageAsync(messageDto, conversationId, userId.Value);
        
        if (message == null)
        {
            return ServiceResult<ResponseMessageDTO>.Fail("Failed to send message.");
        }

        var responseMessage = new ResponseMessageDTO
        {
            Id = message.Id,
            Content = message.Content,
            SenderId = message.SenderId,
            SentAt = message.SentAt
        };

        return ServiceResult<ResponseMessageDTO>.Success(responseMessage);
    }

    public async Task<ServiceResult<List<ResponseMessageDTO>>> GetMessagesInConversationAsync(
        Guid conversationId, 
        int take = 20, 
        DateTime? before = null)
    {
        var userId = GetCurrentUserId();
        if (!userId.HasValue)
        {
            return ServiceResult<List<ResponseMessageDTO>>.Fail("User is not authenticated.");
        }

        if (!await IsUserInConversation(conversationId, userId.Value))
        {
            return ServiceResult<List<ResponseMessageDTO>>.Fail("User is not part of this conversation.");
        }

        var messages = await _repo.GetMessagesInConversationAsync(conversationId, take, before);
        return ServiceResult<List<ResponseMessageDTO>>.Success(messages);
    }

    public async Task<ServiceResult<ResponseMessageDTO>> UpdateMessageAsync(Guid messageId, string newContent)
    {
        var userId = GetCurrentUserId();
        if (!userId.HasValue)
        {
            return ServiceResult<ResponseMessageDTO>.Fail("User is not authenticated.");
        }

        var message = await _repo.GetMessageByIdAsync(messageId);
        if (message == null)
        {
            return ServiceResult<ResponseMessageDTO>.Fail("Message not found.");
        }

        if (message.SenderId != userId)
        {
            return ServiceResult<ResponseMessageDTO>.Fail("You can only edit your own messages.");
        }

        message = await _repo.UpdateMessageAsync(messageId, newContent);
        
        var responseMessage = new ResponseMessageDTO
        {
            Id = message.Id,
            Content = message.Content,
            SenderId = message.SenderId,
            SentAt = message.SentAt
        };

        return ServiceResult<ResponseMessageDTO>.Success(responseMessage);
    }

    public async Task<ServiceResult<bool>> DeleteMessageAsync(Guid messageId)
    {
        var userId = GetCurrentUserId();
        if (!userId.HasValue)
        {
            return ServiceResult<bool>.Fail("User is not authenticated.");
        }

        var message = await _repo.GetMessageByIdAsync(messageId);
        if (message == null)
        {
            return ServiceResult<bool>.Fail("Message not found.");
        }

        if (message.SenderId != userId)
        {
            return ServiceResult<bool>.Fail("You can only delete your own messages.");
        }

        var result = await _repo.DeleteMessageAsync(messageId);
        return result ? 
            ServiceResult<bool>.Success(true) : 
            ServiceResult<bool>.Fail("Failed to delete message.");
    }
}