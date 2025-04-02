using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.Models.DTOs;
using Server.Repository.IRepo;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        [HttpGet("GetGoal/{goalId}")]
        public async Task<IActionResult> getGoal(int goalId)
        {
            var res = await _goalRepo.getGoal(goalId);
            return Ok(res);
        }

        [Authorize]
        [HttpGet("GetAllGoals/{pageNumber}")]
        public async Task<IActionResult> getAllGoals(int pageNumber,[FromQuery] int pageSize, [FromQuery] string? search, [FromQuery] int? statusId)
        {
            var token = Request.Headers.Authorization.ToString().Split(" ")[1];
            var data = await _goalRepo.getAllGoals(token, search, statusId);

            int total = data.Count();
            int totalPage = (int)Math.Ceiling(total / (double)pageSize);

            var result = data.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            var response = new
            {
                Data = result,
                PageNumber = total == 0 ? 0 : pageNumber,
                TotalPage = totalPage,
            };
            return Ok(response);
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
        [HttpPut("EditGoal/{goalId}")]
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
