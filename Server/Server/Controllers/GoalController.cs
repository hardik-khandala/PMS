using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.Models.DTOs;
using Server.Repository.IRepo;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoalController : ControllerBase
    {
        private readonly IGoalRepo _goalRepo;
        public GoalController(IGoalRepo goalRepo)
        {
            _goalRepo = goalRepo;
        }

        [Authorize]
        [HttpGet("GetAllGoals")]
        public async Task<IActionResult> getAllGoals()
        {
            var token = Request.Headers.Authorization.ToString().Split(" ")[1];
            var res = await _goalRepo.getAllGoals(token);

            return Ok(res);
        }

        [Authorize]
        [HttpPost("AddGoal")]
        public async Task<IActionResult> addGoal(GoalDTO goal)
        {
            var token = Request.Headers.Authorization.ToString().Split(" ")[1];
            var res = await _goalRepo.addGoal(token, goal);
            if (res)
            {
                return Ok(new { msg = "Goal Added!" });
            }
            return BadRequest(new { msg = "Something went wrong!!" });
        }

        [Authorize]
        [HttpPut("EditGoal")]
        public async Task<IActionResult> editGoal(int goalId, GoalDTO goal)
        {
            var token = Request.Headers.Authorization.ToString().Split(" ")[1];
            var res = await _goalRepo.editGoal(token, goalId, goal);

            if (res)
            {
                return Ok(new { msg = "Goal Updated" });
            }
            return BadRequest(new { msg = "Something went wrong!!" });
        }

        [Authorize]
        [HttpDelete("DeleteGoal/{goalId}")]
        public async Task<IActionResult> deleteGoal(int goalId)
        {
            var token = Request.Headers.Authorization.ToString().Split(" ")[1];
            var res = await _goalRepo.removeGoal(token, goalId);

            if (res)
            {
                return Ok(new { msg = "Goal Deleted!" });
            }
            return BadRequest(new { msg = "Something went wrong!!" });
        }
    }
}
