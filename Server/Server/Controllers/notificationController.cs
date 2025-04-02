using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.Repository.IRepo;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class notificationController : ControllerBase
    {
        private readonly INotificationRepo _notificationRepo;
        public notificationController(INotificationRepo notificationRepo)
        {
            _notificationRepo = notificationRepo;
        }

        [Authorize]
        [HttpGet("GetAllNotification")]
        public async Task<IActionResult> GetAllNotifications()
        {
            var token = Request.Headers.Authorization.ToString().Split(" ")[1];

            var res = await _notificationRepo.GetNotifications(token);
            var count = res.Count;
            return Ok(new {res, count});
        }

        [Authorize]
        [HttpPut("MarkAsRead/{Id}")]
        public async Task<IActionResult> MarkAsRead(int Id)
        {
            var res = await _notificationRepo.MarkAsRead(Id);
            if (res)
            {
                return Ok(new { msg = "Success" });
            }
            return BadRequest(new { msg = "Something went wrong!" });
        }

        [Authorize]
        [HttpPut("ClearAll")]
        public async Task<IActionResult> ClearAll()
        {
            var token = Request.Headers.Authorization.ToString().Split(" ")[1];

            var res = await _notificationRepo.ClearAll(token);

            if (res)
            {
                return Ok(new { msg = "Success" });
            }
            return BadRequest(new { msg = "Something went wrong!" });
        }
    }
}
