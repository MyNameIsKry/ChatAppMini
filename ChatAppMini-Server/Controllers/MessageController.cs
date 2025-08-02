using Microsoft.AspNetCore.Mvc;
using ChatAppMini.Services;
using Microsoft.AspNetCore.Authorization;
using Utils;

namespace ChatAppMini.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MessageController : ControllerBase
{
    private readonly IMessageService _messageService;

    public MessageController(IMessageService messageService) => _messageService = messageService;

    [HttpPost("send/{conversationId}")]
    public async Task<ApiResponse<ResponseMessageDTO>> SendMessageAsync(Guid conversationId, [FromBody] RequestMessageDTO messageDto)
    {
        if (messageDto == null)
        {
            return new ApiResponse<ResponseMessageDTO>(400, "Message content cannot be null.");
        }

        ServiceResult<ResponseMessageDTO> result = await _messageService.SendMessageAsync(messageDto, conversationId);

        if (result.IsSuccess)
            return new ApiResponse<ResponseMessageDTO>(201, "Message sent successfully", result.Data);
        
        return new ApiResponse<ResponseMessageDTO>(400, result.Message);
    }

    [HttpGet("conversation/{conversationId}")]
    public async Task<ApiResponse<List<ResponseMessageDTO>>> GetMessagesAsync(
        Guid conversationId, 
        [FromQuery] int take = 20,
        [FromQuery] DateTime? before = null)
    {
        var result = await _messageService.GetMessagesInConversationAsync(conversationId, take, before);

        if (result.IsSuccess)
            return new ApiResponse<List<ResponseMessageDTO>>(200, "Messages retrieved successfully", result.Data);
        
        return new ApiResponse<List<ResponseMessageDTO>>(400, result.Message);
    }

    [HttpPut("{messageId}")]
    public async Task<ApiResponse<ResponseMessageDTO>> UpdateMessageAsync(
        Guid messageId, 
        [FromBody] string newContent)
    {
        if (string.IsNullOrEmpty(newContent))
        {
            return new ApiResponse<ResponseMessageDTO>(400, "Message content cannot be empty.");
        }

        var result = await _messageService.UpdateMessageAsync(messageId, newContent);

        if (result.IsSuccess)
            return new ApiResponse<ResponseMessageDTO>(200, "Message updated successfully", result.Data);
        
        return new ApiResponse<ResponseMessageDTO>(400, result.Message);
    }

    [HttpDelete("{messageId}")]
    public async Task<ApiResponse<bool>> DeleteMessageAsync(Guid messageId)
    {
        var result = await _messageService.DeleteMessageAsync(messageId);

        if (result.IsSuccess)
            return new ApiResponse<bool>(200, "Message deleted successfully", true);
        
        return new ApiResponse<bool>(400, result.Message);
    }
}