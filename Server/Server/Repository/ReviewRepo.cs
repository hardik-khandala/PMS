using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Server.Hubs;
using Server.Models;
using Server.Models.DTOs;
using Server.Repository.IRepo;
using Server.Services;

namespace Server.Repository
{
    public class ReviewRepo : IReviewRepo
    {
        private readonly AppDbContext _context;
        private readonly TokenService _tokenService;
        private readonly IHubContext<NotificationHub> _hubContext;
        public ReviewRepo(AppDbContext context, TokenService tokenService, IHubContext<NotificationHub> hubContext)
        {
            _context = context;
            _tokenService = tokenService;
            _hubContext = hubContext;
        }
        public async Task<List<selfReviewListDTO>> selfReviewList(string token)
        {
            var list = _tokenService.GetData(token);
            var id = list.FirstOrDefault(e => e.Type == "id")?.Value;
            var res = await _context.PerfomanceReviews.Where(e => e.EmpId == Convert.ToInt32(id) && e.IsDeleted == false)
                .Select(e => new selfReviewListDTO
                {
                    ReviewId = e.ReviewId,
                    ManagerName = e.Manager.EmpName,
                    StartDate = e.StartDate,
                    EndDate = e.EndDate,
                    Title = e.Criteria.CriteriaName,
                    SelfRating = e.SelfRating,
                    ManagerRating = e.ManagerRating,
                    ManagerFeedback = e.ManagerFeedback,
                    Status = e.Status.Status,
                    CreatedAt = (DateTime)e.CreatedAt
                }).ToListAsync();
            return res;
        }

        public async Task<selfReviewDTO> getSelfReview(int id)
        {
            var res = await _context.PerfomanceReviews.FirstOrDefaultAsync(r => r.ReviewId == id);

            var result = new selfReviewDTO
            {
                CriteriaId = res.CriteriaId,
                StartDate = res.StartDate,  
                EndDate = res.EndDate,
                SelfRating = res.SelfRating,
                Improvement = res.Improvement,
                Strength = res.Strength
            };
            return result; 
        }

        public async Task<List<managerReviewList>> managerReviewList(string token)
        {
            var list = _tokenService.GetData(token);
            var id = list.FirstOrDefault(e => e.Type == "id")?.Value;
            var res = await _context.PerfomanceReviews.Include(e => e.Emp).Where(e => e.ManagerId == Convert.ToInt32(id) && e.IsDeleted == false && e.StatusId == 1)
                .Select(e => new managerReviewList
                {
                    ReviewId = e.ReviewId,
                    EmpName = e.Emp.UserName,
                    StartDate = e.StartDate,
                    Title = e.Criteria.CriteriaName,
                    EndDate = e.EndDate,
                    SelfRating = e.SelfRating,
                    Strength = e.Strength,
                    Improvement = e.Improvement,
                    CreatedAt = (DateTime)e.CreatedAt
                }).ToListAsync();

            return res;
        }

        public async Task<bool> selfReview(string token, selfReviewDTO selfReview)
        {
            try
            {
                var list = _tokenService.GetData(token);
                var id = list.FirstOrDefault(e => e.Type == "id")?.Value;
                var managerId = _context.Employees.FirstOrDefaultAsync(e => e.EmpId == Convert.ToInt32(id)).Result?.ManagerId;
                var review = new PerfomanceReview
                {
                    EmpId = Convert.ToInt32(id),
                    ManagerId = Convert.ToInt32(managerId),
                    Improvement = selfReview.Improvement,
                    Strength = selfReview.Strength,
                    SelfRating = selfReview.SelfRating,
                    StartDate = selfReview.StartDate,
                    EndDate = selfReview.EndDate,
                    CreatedBy = Convert.ToInt32(id),
                    StatusId = 1,
                    CriteriaId = selfReview.CriteriaId
                };
                await _context.PerfomanceReviews.AddAsync(review);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> managerReview(string token, managerReviewDTO managerReview)
        {
            try
            {
                var list = _tokenService.GetData(token);
                var ManagerId = list.FirstOrDefault(e => e.Type == "id")?.Value;
                var review = await _context.PerfomanceReviews.FirstOrDefaultAsync(e => e.ReviewId == managerReview.ReviewId);
                if (review is null)
                {
                    return false;
                }
                review.ManagerRating = managerReview.ManagerRating;
                review.ManagerFeedback = managerReview.ManagerFeedback;
                review.ModifyAt = DateTime.UtcNow;
                review.StatusId = 2;
                review.ModifyBy = Convert.ToInt32(ManagerId);
                //await _context.SaveChangesAsync();

                string empId = review.EmpId.ToString();
                string message = $"Your Review for {review.Criteria.CriteriaName} has been Approve by {review.Manager.EmpName}";

                var notification = new Notification
                {
                    CreatedAt = DateTime.UtcNow,
                    EmpId = review.EmpId,
                    Message = message,
                    IsRead = false,
                };

                await _context.Notifications.AddAsync(notification);
                await _context.SaveChangesAsync();

                await _hubContext.Clients.User(empId).SendAsync("ReceiveNotification", message);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> managerRevise(string token, int id)
        {
            try
            {
                var list = _tokenService.GetData(token);
                var ManagerId = list.FirstOrDefault(e => e.Type == "id")?.Value;
                var data = await _context.PerfomanceReviews.FirstOrDefaultAsync(e => e.ReviewId == id);

                if (data is null)
                {
                    return false;
                }
                data.ModifyBy = Convert.ToInt32(ManagerId);
                data.ModifyAt = DateTime.UtcNow;
                data.StatusId = 3;
                await _context.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> editSelfReview(int id, selfReviewDTO selfReview)
        {
            try
            {
                var review = await _context.PerfomanceReviews.FirstOrDefaultAsync(r => r.ReviewId == id);
                if (review is null)
                {
                    return false;
                }
                review.StartDate = selfReview.StartDate;
                review.EndDate = selfReview.EndDate;
                review.StatusId = 1;
                review.Improvement = selfReview.Improvement;
                review.SelfRating = selfReview.SelfRating; 
                review.Strength = selfReview.Strength;
                review.ModifyAt = DateTime.UtcNow;
                review.ModifyBy = review.EmpId;

                await _context.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> deleteSelfReview(int id)
        {
            var review = await _context.PerfomanceReviews.FirstOrDefaultAsync(e => e.ReviewId == id);
            if (review == null)
            {
                return false;
            }
            review.IsDeleted = true;
            await _context.SaveChangesAsync();
            return true;
        }

       
    }
}
