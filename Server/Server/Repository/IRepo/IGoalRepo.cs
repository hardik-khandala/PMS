using Server.Models;
using Server.Models.DTOs;

namespace Server.Repository.IRepo
{
    public interface IGoalRepo
    {
        Task<List<GoalListDTO>> getAllGoals(string token);
        Task<bool> addGoal(string token, GoalDTO goal);
        Task<bool> editGoal(string token,int goalId, GoalDTO goal);
        Task<bool> removeGoal(string token, int goalId);
    }
}
