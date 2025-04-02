using Microsoft.AspNetCore.SignalR;
using Server.Repository.IRepo;
using System.Collections.Concurrent;

namespace Server.Hubs
{
    public class NotificationHub : Hub
    {
        public static ConcurrentDictionary<string, string> _connections = new();
        private readonly INotificationRepo _notificationRepo;
        public NotificationHub(INotificationRepo notificationRepo)
        {
            _notificationRepo = notificationRepo;
        }
        public override Task OnConnectedAsync()
        {
            string userId = Context.User.Claims.FirstOrDefault(e=>e.Type == "id")?.Value;
            if (userId != null)
            {
                _connections[userId] = Context.ConnectionId;
            }

            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception? exception)
        {
            string userId = Context.User.Claims.FirstOrDefault(e => e.Type == "id")?.Value;

            if (userId != null)
            {
                _connections.TryRemove(userId, out _);
            }

            return base.OnDisconnectedAsync(exception);
        }
        public async Task SendNotification(string userId, string message)
        {
            if (_connections.TryGetValue(userId, out string connectionId))
            {   
                await Clients.Client(connectionId).SendAsync("ReceiveNotification", message);
            }
        }
    }
}
