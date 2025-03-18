using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.Models.DTOs;
using Server.Repository.IRepo;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class authController : ControllerBase
    {
        private readonly IAuthRepo _authRepo;
        public authController(IAuthRepo authRepo)
        {
            _authRepo = authRepo;
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDTO login)
        {
            var token = await _authRepo.Login(login);
            if (token == string.Empty)
            {
                return BadRequest("Invalid Username Or Password");

            }
            return Ok(new { token });

        }
        [Authorize(Roles = "Admin")]
        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDTO register)
        {
            var token = Request.Headers.Authorization.ToString().Split(" ")[1];
            bool res = await _authRepo.Register(token, register);
            if (res)
            {
                return Ok(new { Message = "User Register Successfully" });
            }
            return BadRequest(new { ErrorMessage = "Something went wrong" });
        }
    }
}
