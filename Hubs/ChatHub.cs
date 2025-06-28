using Microsoft.AspNetCore.SignalR;
using ChatAppMini.Models;

namespace ChatAppMini.Hubs;

public class ChatHub : Hub
{
    public async Task SendMessage(MessageDto message)
    {
        await Clients.All.SendAsync("ReceiveMessage", message);
    }
}
