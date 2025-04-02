using Microsoft.EntityFrameworkCore;
using Server.Models;
using Server.Models.DTOs;
using Server.Repository.IRepo;
using Server.Services;

namespace Server.Repository
{
    public class NotificationRepo : INotificationRepo
    {
        private readonly AppDbContext _context;
        private readonly TokenService _tokenService;
        public NotificationRepo(AppDbContext context, TokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }
        public async Task<bool> MarkAsRead(int notificationId)
        {
            try
            {
                var noti = await _context.Notifications.FirstOrDefaultAsync(n => n.NotificationId == notificationId);
                noti.IsRead = true;
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> ClearAll(string token)
        {
            try
            {
                var list = _tokenService.GetData(token);
                var empId = list.FirstOrDefault(e => e.Type == "id")?.Value;

                var notifications = await _context.Notifications.Where(n => n.EmpId == Convert.ToInt32(empId) && n.IsRead == false).ToListAsync();
                foreach (var item in notifications)
                {
                    item.IsRead = true;
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<notificationDTO>> GetNotifications(string token)
        {
            var list = _tokenService.GetData(token);
            var empId = list.FirstOrDefault(e => e.Type == "id")?.Value;

            var notifications = await _context.Notifications.Where(n => n.EmpId == Convert.ToInt32(empId) && n.IsRead == false)
                .Select(n => new notificationDTO
                {
                    NotificationId = n.NotificationId,
                    Message = n.Message,
                    CreatedAt = n.CreatedAt
                }).ToListAsync();

            return notifications;
        }

    }
}
