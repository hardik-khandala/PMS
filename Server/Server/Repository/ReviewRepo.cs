using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Server.Hubs;
using Server.Models;
using Server.Models.DTOs;
using Server.Models.Enum;
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
        public async Task<List<selfReviewListDTO>> selfReviewList(string token, string? search, int? statusId)
        {
            var list = _tokenService.GetData(token);
            var id = list.FirstOrDefault(e => e.Type == "id")?.Value;
            var data = _context.PerfomanceReviews.Where(e => e.EmpId == Convert.ToInt32(id) && e.IsDeleted == false);
            if (statusId.HasValue && statusId > 0)
            {
                data = data.Where(r => r.StatusId == statusId);
            };
            if (!search.IsNullOrEmpty())
            {
                data = data.Where(e => e.Manager.EmpName.ToLower().Contains(search.ToLower()) || e.Criteria.CriteriaName.ToLower().Contains(search.ToLower()) || e.ReviewId.ToString().Contains(search));
            };
            var res = await data.Select(e => new selfReviewListDTO
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

        public async Task<List<managerReviewList>> managerReviewList(string token, string? search, int? rating)
        {
            var list = _tokenService.GetData(token);
            var id = list.FirstOrDefault(e => e.Type == "id")?.Value;
            var data = _context.PerfomanceReviews.Include(e => e.Emp).Where(e => e.ManagerId == Convert.ToInt32(id) && e.IsDeleted == false && e.StatusId == 1);


            if (rating.HasValue && rating > 0)
            {
                data = data.Where(r => r.SelfRating == rating);
            };
            if (!search.IsNullOrEmpty())
            {
                data = data.Where(e => e.Emp.EmpName.ToLower().Contains(search.ToLower()) || e.Criteria.CriteriaName.ToLower().Contains(search.ToLower()));
            };

            var res = await data.Select(e => new managerReviewList
            {
                ReviewId = e.ReviewId,
                EmpName = e.Emp.EmpName,
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
                    StatusId = Convert.ToInt32(Status.Pending),
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
                var review = await _context.PerfomanceReviews.Include(c => c.Criteria).Include(c => c.Manager).FirstOrDefaultAsync(e => e.ReviewId == managerReview.ReviewId);
                if (review is null)
                {
                    return false;
                }
                review.ManagerRating = managerReview.ManagerRating;
                review.ManagerFeedback = managerReview.ManagerFeedback;
                review.ModifyAt = DateTime.Now;
                review.StatusId = Convert.ToInt32(Status.Approve);
                review.ModifyBy = Convert.ToInt32(ManagerId);
                //await _context.SaveChangesAsync();

                string empId = review.EmpId.ToString();
                string message = $"Your Review for {review.Criteria.CriteriaName} has been Approve by {review.Manager.EmpName}";

                var notification = new Notification
                {
                    CreatedAt = DateTime.Now,
                    EmpId = review.EmpId,
                    Message = message,
                    IsRead = false,
                };

                await _context.Notifications.AddAsync(notification);
                await _context.SaveChangesAsync();

                NotificationHub._connections.TryGetValue(empId, out string connectionId);
                if (connectionId.IsNullOrEmpty() == false)
                {
                    await _hubContext.Clients.Client(connectionId).SendAsync("ReceiveNotification", message);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> managerRevise(string token, int id, string managerFeedback)
        {
            try
            {
                var list = _tokenService.GetData(token);
                var ManagerId = list.FirstOrDefault(e => e.Type == "id")?.Value;
                var data = await _context.PerfomanceReviews.Include(e => e.Criteria).Include(e => e.Manager).FirstOrDefaultAsync(e => e.ReviewId == id);

                if (data is null)
                {
                    return false;
                }
                data.ModifyBy = Convert.ToInt32(ManagerId);
                data.ModifyAt = DateTime.Now;
                data.StatusId = Convert.ToInt32(Status.Revision);
                data.ManagerFeedback = managerFeedback;

                var empId = data.EmpId.ToString();
                var message = $"Your Review for {data.Criteria.CriteriaName} has been Rejected by {data.Manager.EmpName}";

                var notification = new Notification
                {
                    CreatedAt = DateTime.Now,
                    EmpId = data.EmpId,
                    Message = message,
                    IsRead = false,
                };
                await _context.Notifications.AddAsync(notification);
                await _context.SaveChangesAsync();

                NotificationHub._connections.TryGetValue(empId, out string connectionId);
                if (connectionId.IsNullOrEmpty() == false)
                {
                    await _hubContext.Clients.Client(connectionId).SendAsync("ReceiveNotification", message);
                }
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
                review.StatusId = Convert.ToInt32(Status.Pending);
                review.Improvement = selfReview.Improvement;
                review.SelfRating = selfReview.SelfRating;
                review.Strength = selfReview.Strength;
                review.ModifyAt = DateTime.Now;
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
