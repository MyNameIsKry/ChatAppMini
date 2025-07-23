using Microsoft.AspNetCore.Mvc;
using Utils;
using ChatAppMini.DTOs.Conversation;
using ChatAppMini.Services.Conversations;
using Microsoft.AspNetCore.Authorization;

namespace ChatAppMini.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ConversationController : ControllerBase
{
    private readonly IConversationService _conversationService;

    public ConversationController(IConversationService conversationService) => _conversationService = conversationService;

    [HttpGet("{id}")]
    [Authorize]
    public async Task<ApiResponse<ResponseConversationDTO>> GetConversationAsync(Guid id)
    {
        ServiceResult<ResponseConversationDTO?> result = await _conversationService.GetConversationAsync(id);

        if (result.IsSuccess)
            return new ApiResponse<ResponseConversationDTO>(200, result.Message, result.Data);

        return new ApiResponse<ResponseConversationDTO>(500, result.Message);
    }

    [HttpPost]
    [Authorize]
    public async Task<ApiResponse<Conversation>> CreateConversationAsync([FromBody] Guid userId)
    {
        ServiceResult<Conversation> result = await _conversationService.CreateConversationAsync(userId);

        if (result.IsSuccess)
            return new ApiResponse<Conversation>(201, result.Message, result.Data);

        return new ApiResponse<Conversation>(500, result.Message);
    }
}