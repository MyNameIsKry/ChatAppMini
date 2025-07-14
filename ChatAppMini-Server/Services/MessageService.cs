using ChatAppMini.Models;
using ChatAppMini.DTOs.User;
using Utils;
using System.Security.Claims;

namespace ChatAppMini.Services;

public interface IMessageService
{
    Task<ServiceResult<Message>> SaveUsersMessages(RequestMessageDTO msg);
    Task<ServiceResult<List<Message>>> GetUsersMessages(Guid withUser);
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

    public async Task<ServiceResult<Message>> SaveUsersMessages(RequestMessageDTO msg)
    {
        if (msg.Content.Length == 0)
            return ServiceResult<Message>.Fail("Content must not empty");

        Message message = await _repo.SaveUsersMessages(msg);
        await _repo.SaveChangesAsync();
        return ServiceResult<Message>.Success(message);
    }

    public async Task<ServiceResult<List<Message>>> GetUsersMessages(Guid withUser)
    {
        var userId = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
            return ServiceResult<List<Message>>.Fail("User Id empty!");

        if (!Guid.TryParse(userId, out var guidUserId))
            return ServiceResult<List<Message>>.Fail("Invalid User Id format!");

        List<Message> listMessages = await _repo.GetUsersMessages(guidUserId, withUser);

        return ServiceResult<List<Message>>.Success(listMessages);
    }
}