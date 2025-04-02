using Server.Models;

namespace Server.Services
{
    public class DueDateService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        public DueDateService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await CheckGoalsAndAddNotifications();
                await Task.Delay(TimeSpan.FromDays(1), stoppingToken); 
            }
        }
        private async Task CheckGoalsAndAddNotifications()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var tomorrow = DateOnly.FromDateTime(DateTime.Today).AddDays(1);

       
                var goalsDueTomorrow = dbContext.Goals
                    .Where(g => g.DueDate == tomorrow && g.StatusId == 1)
                    .ToList();

                foreach (var goal in goalsDueTomorrow)
                {
                    var notification = new Notification
                    {
                        EmpId = goal.EmpId,
                        CreatedAt = DateTime.Now,
                        IsRead = false,
                        Message = $"Your Goal: '{goal.Title}' is due tomorrow!",
                    };
                    dbContext.Notifications.Add(notification);

                }

                await dbContext.SaveChangesAsync();
            }
        }
    }
}
