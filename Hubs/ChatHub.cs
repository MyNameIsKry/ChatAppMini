using Microsoft.AspNetCore.SignalR;
using ChatAppMini.Models;
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

        await Clients.All.SendAsync("UserConnected", $"Người dùng {userId} đã kết nối");
        await base.OnConnectedAsync();
    }

    public async Task SendMessage(string message)
    {
        string? username = Context?.User?.Identity?.Name;

        await Clients.All.SendAsync("ReceiveMessage", username, message);
    }
}
