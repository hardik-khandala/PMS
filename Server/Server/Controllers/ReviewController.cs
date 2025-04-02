using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.Models.DTOs;
using Server.Repository.IRepo;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewRepo _reviewRepo;
        public ReviewController(IReviewRepo reviewRepo)
        {
            _reviewRepo = reviewRepo;
        }
        [Authorize]
        [HttpGet("SelfReviewList/{pageNumber}")]
        public async Task<IActionResult> selfReviewList(int pageNumber,[FromQuery] int pageSize, [FromQuery] string? search,[FromQuery] int? statusId)
        {
            var token = Request.Headers.Authorization.ToString().Split(" ")[1];
            var data = await _reviewRepo.selfReviewList(token, search, statusId);

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
        [HttpGet("getReview")]
        public async Task<IActionResult> getReview(int id)
        {   
            var res = await _reviewRepo.getSelfReview(id);
            return Ok(res); 
        }

        [Authorize]
        [HttpGet("managerReviewList/{pageNumber}")]
        public async Task<IActionResult> managerReviewList(int pageNumber, [FromQuery] string? search, [FromQuery] int? rating)
        {
            var token = Request.Headers.Authorization.ToString().Split(" ")[1];
            var data = await _reviewRepo.managerReviewList(token, search, rating);

            int total = data.Count();
            int pageSize = 5;
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
        [HttpPost("SelfReview")]
        public async Task<IActionResult> selfReview([FromBody] selfReviewDTO selfReviewDTO)
        {
            var token = Request.Headers.Authorization.ToString().Split(" ")[1];
            var review = await _reviewRepo.selfReview(token, selfReviewDTO);
            if (review)
            {
                return Ok(new { msg = "Self Review Added" });
            }
            return BadRequest(new { msg = "Something went Wrong!!" });
        }

        [Authorize]
        [HttpPut("ManagerReview")]
        public async Task<IActionResult> managerReview(managerReviewDTO managerReview)
        {
            var token = Request.Headers.Authorization.ToString().Split(" ")[1];
            var res = await _reviewRepo.managerReview(token, managerReview);
            if (res)
            {
                return Ok(new { msg = "Manager Review Added" });
            }
            return BadRequest(new { msg = "Something went Wrong!!" });
        }

        [Authorize]
        [HttpPut("ManagerRevise/{id}")]
        public async Task<IActionResult> managerRevise(int id,[FromQuery] string managerFeedback)
        {
            var token = Request.Headers.Authorization.ToString().Split(" ")[1];
            var res = await _reviewRepo.managerRevise(token, id, managerFeedback);
            if (res)
            {
                return Ok(new { msg = "Submit for Revise Review" });
            }
            return BadRequest(new { msg = "Something went Wrong!!" });
        }

        [Authorize]
        [HttpPut("EditSelfReview/{id}")]
        public async Task<IActionResult> EditSelfReview(int id, selfReviewDTO selfreview)
        {
            var res = await _reviewRepo.editSelfReview(id, selfreview);
            if (res)
            {
                return Ok(new { msg = "Review Edited Successfully" });
            }
            return BadRequest(new { msg = "Something went Wrong!!" });
        }

        [Authorize]
        [HttpDelete("deleteSelfReview/{id}")]
        public async Task<IActionResult> deleteSelfReview(int id)
        {
            var data = await _reviewRepo.deleteSelfReview(id);
            if (data)
            {
                return Ok(new { msg = "Self Review Deleted Successfully" });
            }
            return BadRequest(new { msg = "Something went Wrong!!" });
        }
    }
}
