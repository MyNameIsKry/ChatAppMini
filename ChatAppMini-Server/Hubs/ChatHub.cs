using Microsoft.AspNetCore.SignalR;
using Utils;
using Microsoft.AspNetCore.Authorization;
using ChatAppMini.Services;
using ChatAppMini.Services.Conversations;
using ChatAppMini.DTOs.Message;
using ChatAppMini.DTOs.Conversation;
using System.Security.Claims;
using ChatAppMini.DTOs.User;

namespace ChatAppMini.Hubs;

[Authorize]
public class ChatHub : Hub
{
    private readonly IMessageService _messageService;
    private readonly IConversationService _conversationService;
    private static readonly Dictionary<string, string> UserConnections = new();

    public ChatHub(IMessageService messageService, IConversationService conversationService)
    {
        _messageService = messageService;
        _conversationService = conversationService;
    }

    public override async Task OnConnectedAsync()
    {
        string? userId = Context?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string? userName = Context?.User?.FindFirst(ClaimTypes.Name)?.Value;

        if (!string.IsNullOrEmpty(userId) && Context != null)
        {
            UserConnections[userId] = Context.ConnectionId;
            Logger.Log($"User connected: {userName} ({userId}) with connection ID: {Context.ConnectionId}");
        }
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        string? userId = Context?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string? userName = Context?.User?.FindFirst(ClaimTypes.Name)?.Value;

        if (!string.IsNullOrEmpty(userId) && UserConnections.ContainsKey(userId))
        {
            UserConnections.Remove(userId);
            Logger.Log($"User disconnected: {userName} ({userId})");
        }
        await base.OnDisconnectedAsync(exception);
    }

    // Gửi tin nhắn trong cuộc trò chuyện (1-1 hoặc nhóm)
    public async Task SendMessage(string content, string conversationId)
    {
        string? senderId = Context?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string? senderName = Context?.User?.FindFirst(ClaimTypes.Name)?.Value;

        if (string.IsNullOrEmpty(senderId))
        {
            throw new HubException("User not authenticated");
        }

        try
        {
            // Tạo và lưu tin nhắn
            var messageDto = new RequestMessageDTO
            {
                Content = content
            };

            var result = await _messageService.SendMessageAsync(messageDto, Guid.Parse(conversationId));
            if (!result.IsSuccess)
            {
                throw new HubException(result.Message);
            }

            // Lấy thông tin cuộc trò chuyện
            var conversationResult = await _conversationService.GetConversationAsync(Guid.Parse(conversationId));
            if (!conversationResult.IsSuccess || conversationResult.Data == null)
            {
                throw new HubException(conversationResult.Message);
            }

            // Gửi tin nhắn đến tất cả người tham gia
            foreach (var participant in conversationResult.Data.Participants)
            {
                if (UserConnections.TryGetValue(participant.UserId.ToString(), out string? connectionId) && 
                    !string.IsNullOrEmpty(connectionId))
                {
                    await Clients.Client(connectionId).SendAsync("ReceiveMessage", result.Data);
                }
            }

            // Ghi log
            Logger.Log($"Message sent in conversation {conversationId} by {senderName} ({senderId}): {content}");
        }
        catch (Exception ex)
        {
            Logger.LogError("Error in SendMessage", ex);
            throw new HubException($"Failed to process message: {ex.Message}");
        }
    }

    // Thông báo khi có người tham gia cuộc trò chuyện
    public async Task JoinConversation(string conversationId)
    {
        string? userId = Context?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string? userName = Context?.User?.FindFirst(ClaimTypes.Name)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            throw new HubException("User not authenticated");
        }

        try
        {
            var conversationResult = await _conversationService.GetConversationAsync(Guid.Parse(conversationId));
            if (!conversationResult.IsSuccess || conversationResult.Data == null)
            {
                throw new HubException(conversationResult.Message);
            }

            // Thông báo cho các thành viên khác
            foreach (var participant in conversationResult.Data.Participants)
            {
                if (participant.UserId.ToString() != userId && 
                    UserConnections.TryGetValue(participant.UserId.ToString(), out string? connectionId) &&
                    !string.IsNullOrEmpty(connectionId))
                {
                    await Clients.Client(connectionId).SendAsync("UserJoined", new 
                    {
                        userId,
                        userName,
                        conversationId
                    });
                }
            }

            Logger.Log($"User {userName} ({userId}) joined conversation {conversationId}");
        }
        catch (Exception ex)
        {
            Logger.LogError("Error in JoinConversation", ex);
            throw new HubException($"Failed to join conversation: {ex.Message}");
        }
    }

    // Thông báo khi có người rời khỏi cuộc trò chuyện
    public async Task LeaveConversation(string conversationId)
    {
        string? userId = Context?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string? userName = Context?.User?.FindFirst(ClaimTypes.Name)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            throw new HubException("User not authenticated");
        }

        try
        {
            var conversationResult = await _conversationService.GetConversationAsync(Guid.Parse(conversationId));
            if (!conversationResult.IsSuccess || conversationResult.Data == null)
            {
                throw new HubException(conversationResult.Message);
            }

            // Thông báo cho các thành viên khác
            foreach (var participant in conversationResult.Data.Participants)
            {
                if (participant.UserId.ToString() != userId &&
                    UserConnections.TryGetValue(participant.UserId.ToString(), out string? connectionId) &&
                    !string.IsNullOrEmpty(connectionId))
                {
                    await Clients.Client(connectionId).SendAsync("UserLeft", new
                    {
                        userId,
                        userName,
                        conversationId
                    });
                }
            }

            Logger.Log($"User {userName} ({userId}) left conversation {conversationId}");
        }
        catch (Exception ex)
        {
            Logger.LogError("Error in LeaveConversation", ex);
            throw new HubException($"Failed to leave conversation: {ex.Message}");
        }
    }

    // Thông báo khi có người đang nhập tin nhắn
    public async Task SendTypingNotification(string conversationId)
    {
        string? userId = Context?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string? userName = Context?.User?.FindFirst(ClaimTypes.Name)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            throw new HubException("User not authenticated");
        }

        try
        {
            var conversationResult = await _conversationService.GetConversationAsync(Guid.Parse(conversationId));
            if (!conversationResult.IsSuccess || conversationResult.Data == null)
            {
                throw new HubException(conversationResult.Message);
            }

            // Gửi thông báo đến các thành viên khác
            foreach (var participant in conversationResult.Data.Participants)
            {
                if (participant.UserId.ToString() != userId &&
                    UserConnections.TryGetValue(participant.UserId.ToString(), out string? connectionId) &&
                    !string.IsNullOrEmpty(connectionId))
                {
                    await Clients.Client(connectionId).SendAsync("UserTyping", new
                    {
                        userId,
                        userName,
                        conversationId
                    });
                }
            }
        }
        catch (Exception ex)
        {
            Logger.LogError("Error in SendTypingNotification", ex);
            throw new HubException($"Failed to send typing notification: {ex.Message}");
        }
    }
}
