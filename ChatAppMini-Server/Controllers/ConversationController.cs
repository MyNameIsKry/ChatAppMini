using Microsoft.AspNetCore.Mvc;
using Utils;
using ChatAppMini.DTOs.Conversation;
using ChatAppMini.Services.Conversations;
using Microsoft.AspNetCore.Authorization;

namespace ChatAppMini.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ConversationController : ControllerBase
{
    private readonly IConversationService _conversationService;

    public ConversationController(IConversationService conversationService) => _conversationService = conversationService;

    [HttpGet("{id}")]
    public async Task<ApiResponse<ResponseConversationDTO>> GetConversationAsync(Guid id)
    {
        ServiceResult<ResponseConversationDTO?> result = await _conversationService.GetConversationAsync(id);

        if (result.IsSuccess && result.Data != null)
            return new ApiResponse<ResponseConversationDTO>(200, "Conversation retrieved successfully", result.Data);

        if (result.Data == null)
            return new ApiResponse<ResponseConversationDTO>(404, "Conversation not found");

        return new ApiResponse<ResponseConversationDTO>(400, result.Message);
    }

    [HttpGet]
    public async Task<ApiResponse<List<ResponseConversationDTO>>> GetUserConversationsAsync()
    {
        var result = await _conversationService.GetUserConversationsAsync();

        if (result.IsSuccess)
            return new ApiResponse<List<ResponseConversationDTO>>(200, "Conversations retrieved successfully", result.Data);

        return new ApiResponse<List<ResponseConversationDTO>>(400, result.Message);
    }

    [HttpPost]
    public async Task<ApiResponse<ResponseConversationDTO>> CreateConversationAsync(RequestConversationDTO requestConversation)
    {
        if (requestConversation.Participants == null || !requestConversation.Participants.Any())
        {
            return new ApiResponse<ResponseConversationDTO>(400, "Participants list cannot be empty");
        }

        ServiceResult<ResponseConversationDTO> result = await _conversationService.CreateConversationAsync(requestConversation);

        if (result.IsSuccess)
            return new ApiResponse<ResponseConversationDTO>(201, "Conversation created successfully", result.Data);

        return new ApiResponse<ResponseConversationDTO>(400, result.Message);
    }

    [HttpPost("{conversationId}/participants/{participantId}")]
    public async Task<ApiResponse<bool>> AddParticipantAsync(Guid conversationId, Guid participantId)
    {
        var result = await _conversationService.AddParticipantAsync(conversationId, participantId);

        if (result.IsSuccess)
            return new ApiResponse<bool>(200, "Participant added successfully", true);

        return new ApiResponse<bool>(400, result.Message);
    }

    [HttpDelete("{conversationId}/participants/{participantId}")]
    public async Task<ApiResponse<bool>> RemoveParticipantAsync(Guid conversationId, Guid participantId)
    {
        var result = await _conversationService.RemoveParticipantAsync(conversationId, participantId);

        if (result.IsSuccess)
            return new ApiResponse<bool>(200, "Participant removed successfully", true);

        return new ApiResponse<bool>(400, result.Message);
    }

    [HttpDelete("{conversationId}")]
    public async Task<ApiResponse<bool>> DeleteConversationAsync(Guid conversationId)
    {
        var result = await _conversationService.DeleteConversationAsync(conversationId);

        if (result.IsSuccess)
            return new ApiResponse<bool>(200, "Conversation deleted successfully", true);

        return new ApiResponse<bool>(400, result.Message);
    }
}