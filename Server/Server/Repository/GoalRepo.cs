using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Server.Models;
using Server.Models.DTOs;
using Server.Repository.IRepo;
using Server.Services;

namespace Server.Repository
{
    public class GoalRepo : IGoalRepo
    {
        private readonly AppDbContext _context;
        private readonly TokenService _tokenService;
        public GoalRepo(AppDbContext context, TokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }
        public async Task<bool> addGoal(string token, GoalDTO goal)
        {
            try
            {
                var list = _tokenService.GetData(token);
                var id = list.FirstOrDefault(e => e.Type == "id")?.Value;

                var data = new Goal
                {
                    EmpId = Convert.ToInt32(id),
                    Title = goal.Title,
                    Detail = goal.Detail,
                    CreatedAt = DateOnly.FromDateTime(DateTime.Now),
                    CreatedBy = Convert.ToInt32(id),
                    StatusId = 1,
                    IsDeleted = false,
                    DueDate = goal.DueDate
                };
                await _context.Goals.AddAsync(data);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> editGoal(string token, int goalId, GoalDTO goal)
        {
            try
            {
                var list = _tokenService.GetData(token);
                var id = list.FirstOrDefault(e => e.Type == "id")?.Value;

                var data = await _context.Goals.FirstOrDefaultAsync(g => g.GoalId == goalId);
                if (data is null)
                {
                    return false;
                }
                data.Title = goal.Title;
                data.Detail = goal.Detail;
                data.StatusId = goal.StatusId;
                data.ModifyAt = DateOnly.FromDateTime(DateTime.Now);
                data.ModifyBy = Convert.ToInt32(id);
                data.DueDate = goal.DueDate;

                await _context.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<GoalListDTO>> getAllGoals(string token, string? search, int? statusId)
        {
            var list = _tokenService.GetData(token);
            var id = list.FirstOrDefault(e => e.Type == "id")?.Value;

            //var data = _context.Goals.Where(g => g.EmpId == Convert.ToInt32(id) && g.IsDeleted == false);
            //if (statusId.HasValue && statusId > 0)
            //{
            //    data = data.Where(r => r.StatusId == statusId);
            //};
            //if (!search.IsNullOrEmpty())
            //{
            //    data = data.Where(e => e.Title.ToLower().Contains(search.ToLower()));
            //};

            //var res = await data
            //    .Select(g => new GoalListDTO
            //    {
            //        GoalId = g.GoalId,
            //        Title = g.Title,
            //        Detail = g.Detail,
            //        Status = g.Status.Status,
            //        CreatedAt = g.CreatedAt,
            //        DueDate = (DateOnly)g.DueDate
            //    }).ToListAsync();


            var parameters = new[]
            {
                new SqlParameter("@Search", search ?? (object)DBNull.Value),
                new SqlParameter("@StatusId", statusId ?? (object)DBNull.Value),
                new SqlParameter("@EmpId", Convert.ToInt32(id))
            };

            var res = await _context.GoalList
                .FromSqlRaw("EXEC GetGoalData @Search, @StatusId, @EmpId", parameters)
                .ToListAsync();

            return res;
        }

        public async Task<GoalDTO> getGoal(int goalId)
        {
            var goal = await _context.Goals.FirstOrDefaultAsync(g => g.GoalId == goalId);
            var res = new GoalDTO
            {
                Title = goal.Title,
                Detail = goal.Detail,
                StatusId = goal.StatusId,
                DueDate = (DateOnly)goal.DueDate
            };

            return res;
        }

        public async Task<bool> removeGoal(string token, int goalId)
        {
            try
            {
                var list = _tokenService.GetData(token);
                var id = list.FirstOrDefault(e => e.Type == "id")?.Value;

                var goal = await _context.Goals.FirstOrDefaultAsync(g => g.GoalId == goalId);
                if (goal is null)
                {
                    return false;
                }
                goal.IsDeleted = true;
                goal.ModifyAt = DateOnly.FromDateTime(DateTime.Now);
                goal.ModifyBy = Convert.ToInt32(id);
                await _context.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
