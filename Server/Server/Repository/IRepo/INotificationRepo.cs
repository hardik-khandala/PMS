using Server.Models.DTOs;

namespace Server.Repository.IRepo
{
    public interface INotificationRepo
    {
        Task<bool> MarkAsRead(int notificationId);
        Task<bool> ClearAll(string token);
        Task<List<notificationDTO>> GetNotifications(string token);
    }
}
