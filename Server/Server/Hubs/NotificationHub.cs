using Microsoft.AspNetCore.SignalR;

namespace Server.Hubs
{
    public class NotificationHub : Hub
    {

        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception? exception)
        {
            return base.OnDisconnectedAsync(exception);
        }
        public async Task SendNotification(string userId,  string message)
        {
            await Clients.User(userId).SendAsync("ReceiveNotification", message);
        }
    }
}
