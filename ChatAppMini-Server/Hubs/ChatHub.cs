using Microsoft.AspNetCore.SignalR;
using Utils;
using Microsoft.AspNetCore.Authorization;

namespace ChatAppMini.Hubs;

[Authorize]
public class ChatHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        string? userId = Context?.User?.Identity?.Name;
        Logger.Log($"Người dùng đã kết nối: {userId}");

        await Clients.All.SendAsync("MessageSystem", $"{userId} đã tham gia phòng chat");
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        string? userId = Context?.User?.Identity?.Name;
        Logger.Log($"Người dùng đã rời đi: {userId}");

        await Clients.All.SendAsync("MessageSystem", $"{userId} đã rời khỏi phòng chat");
        await base.OnDisconnectedAsync(exception);
    }

    public async Task SendMessage(string message)
    {
        string? sender = Context.User?.Identity?.Name;
        string logMessage = $"{sender}: {message}";

        await Clients.All.SendAsync("MessageSystem", logMessage);
    }
}
