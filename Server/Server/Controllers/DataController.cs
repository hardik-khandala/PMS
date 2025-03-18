using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.Models.DTOs;
using Server.Repository.IRepo;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        private readonly IDataRepo _dataRepo;
        public DataController(IDataRepo dataRepo)
        {
            _dataRepo = dataRepo;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("getAllDept")]
        public async Task<IActionResult> getAllDept()
        {
            var res = await _dataRepo.getDepartment();
            return Ok(res);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("getAllRole")]
        public async Task<IActionResult> getAllRole()
        {
            var res = await _dataRepo.getRole();
            return Ok(res);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("getAllManager")]
        public async Task<IActionResult> getAllManager()
        {
            var res = await _dataRepo.getAllManager();
            return Ok(res);
        }

        [Authorize]
        [HttpGet("getAllCriteria")]
        public async Task<IActionResult> getAllCriteria()
        {
            var res = await _dataRepo.getAllCriteria();
            return Ok(res);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("AddCriteria")]
        public async Task<IActionResult> addCriteria(CriteriaDTO criteria)
        {
            var token = Request.Headers.Authorization.ToString().Split(" ")[1];
            var res = await _dataRepo.AddCriteria(token, criteria);
            if (res)
            {
                return Ok("Criteria Added Successfully");
            }
            return BadRequest();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("AddStatus")]
        public async Task<IActionResult> addStatus(StatusDTO status)
        {
            var token = Request.Headers.Authorization.ToString().Split(" ")[1];
            var res = _dataRepo.AddStatus(token, status);
            return Ok("Status Added Successfully");
        }

    }
}
