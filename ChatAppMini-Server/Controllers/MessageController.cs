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

    [HttpPost("send/{conversationId}")]
    [Authorize]
    public async Task<ApiResponse<ResponseMessageDTO>> SendMessageAsync(Guid conversationId, [FromBody] RequestMessageDTO messageDto)
    {
        if (messageDto == null)
        {
            return new ApiResponse<ResponseMessageDTO>(400, "Message content cannot be null.");
        }

        ServiceResult<ResponseMessageDTO> result = await _messageService.SendMessageAsync(messageDto, conversationId);

        if (result.IsSuccess)
            return new ApiResponse<ResponseMessageDTO>(201, result.Message, result.Data);
        
        return new ApiResponse<ResponseMessageDTO>(500, result.Message);
    }
}