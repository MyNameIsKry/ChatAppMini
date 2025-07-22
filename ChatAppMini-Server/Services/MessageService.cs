using ChatAppMini.DTOs.User;
using Utils;
using System.Security.Claims;

namespace ChatAppMini.Services;

public interface IMessageService
{
    Task<ServiceResult<ResponseMessageDTO>> SendMessageAsync(RequestMessageDTO messageDto, Guid conversationId);
}

public class MessageService : IMessageService
{
    private readonly IMessageRepository _repo;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

    public MessageService(
        IMessageRepository repo,
        IHttpContextAccessor httpContextAccessor
    )
    {
        _repo = repo;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ServiceResult<ResponseMessageDTO>> SendMessageAsync(RequestMessageDTO messageDto, Guid conversationId)
    {
        if (User == null || User?.Identity?.IsAuthenticated != true)
        {
            return ServiceResult<ResponseMessageDTO>.Fail("User is not authenticated.");
        }

        var senderId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var message = await _repo.SendMessageAsync(messageDto, conversationId);
        
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
}