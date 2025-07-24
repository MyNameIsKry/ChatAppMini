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

        if (result.IsSuccess && result.Data != null)
            return new ApiResponse<ResponseConversationDTO>(200, result.Message, result.Data);

        if (result.Data == null)
            return new ApiResponse<ResponseConversationDTO>(404, result.Message);

        return new ApiResponse<ResponseConversationDTO>(500, result.Message);
    }

    [HttpPost]
    [Authorize]
    public async Task<ApiResponse<ResponseConversationDTO>> CreateConversationAsync(RequestConversationDTO requestConversation)
    {
        ServiceResult<ResponseConversationDTO> result = await _conversationService.CreateConversationAsync(requestConversation);

        if (result.IsSuccess)
            return new ApiResponse<ResponseConversationDTO>(201, result.Message, result.Data);

        return new ApiResponse<ResponseConversationDTO>(500, result.Message);
    }
}