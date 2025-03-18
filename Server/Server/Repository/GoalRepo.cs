using Microsoft.EntityFrameworkCore;
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
                    CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow),
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
                if(data is null)
                {
                    return false;
                }
                data.Title = goal.Title;
                data.Detail = goal.Detail;
                data.StatusId = goal.StatusId;
                data.ModifyAt = DateOnly.FromDateTime(DateTime.UtcNow);
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

        public async Task<List<GoalListDTO>> getAllGoals(string token)
        {
            var list = _tokenService.GetData(token);
            var id = list.FirstOrDefault(e => e.Type == "id")?.Value;

            var data = await _context.Goals.Where(g => g.EmpId == Convert.ToInt32(id) && g.IsDeleted == false)
                .Select(g => new GoalListDTO
                {
                    GoalId = g.GoalId,
                    Title = g.Title,
                    Detail = g.Detail,
                    Status = g.Status.Status,
                    CreatedAt = g.CreatedAt,
                    DueDate = (DateOnly)g.DueDate
                }).ToListAsync();

            return data;
        }

        public async Task<bool> removeGoal(string token, int goalId)
        {
            try
            {
                var list = _tokenService.GetData(token);
                var id = list.FirstOrDefault(e => e.Type == "id")?.Value;

                var goal = await _context.Goals.FirstOrDefaultAsync(g => g.GoalId == goalId);
                if(goal is null)
                {
                    return false;
                }
                goal.IsDeleted = true;
                goal.ModifyAt = DateOnly.FromDateTime(DateTime.UtcNow);
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
