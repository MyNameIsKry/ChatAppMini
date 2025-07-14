using Microsoft.AspNetCore.Mvc;
using ChatAppMini.Services;
using Microsoft.AspNetCore.Authorization;
using Utils;

namespace ChatAppMini.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MessageController : ControllerBase
{
    private readonly IMessageService _messageService;

    public MessageController(IMessageService messageService) => _messageService = messageService;

    [Authorize]
    [HttpPost]
    public async Task<ApiResponse<Message>> SaveUsersMessages(RequestMessageDTO msg)
    {
        try
        {
            ServiceResult<Message> msgResult = await _messageService.SaveUsersMessages(msg);

            if (msgResult.IsSuccess)
                return new ApiResponse<Message>(201, "Sent", msgResult.Data);

            return new ApiResponse<Message>(400, msgResult.Message);

        }
        catch (Exception err)
        {
            Logger.LogError("Error saving new messages", err);
            return new ApiResponse<Message>(500, "An error occur when sent messages");
        }
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<ApiResponse<List<Message>>> GetUsersMessages(Guid id)
    {
        try
        {
            ServiceResult<List<Message>> messagesResult = await _messageService.GetUsersMessages(id);
            if (!messagesResult.IsSuccess)
                return new ApiResponse<List<Message>>(400, messagesResult.Message);

            return new ApiResponse<List<Message>>(200, "Messages fetched successfully!", messagesResult.Data);
        }
        catch (Exception err)
        {
            Logger.LogError("Error getting users messages", err);
            return new ApiResponse<List<Message>>(400, "An error occur when get messages");
        }
    }
}