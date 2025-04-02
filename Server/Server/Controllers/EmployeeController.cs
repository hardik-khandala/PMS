using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Models;
using Server.Models.DTOs;
using Server.Repository.IRepo;
using System.Linq;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepo _employeeRepo;

        public EmployeeController(IEmployeeRepo employeeRepo)
        {
            _employeeRepo = employeeRepo;
        }

        [Authorize]
        [HttpGet("getEmp/{id}")]
        public async Task<IActionResult> getEmp(int id)
        {
            var emp = await _employeeRepo.GetEmp(id);
            return Ok(emp);
        }

        [HttpGet("getEmployee/{pageNumber}")]
        public async Task<IActionResult> GetAllEmployees(int pageNumber = 1,[FromQuery] int pageSize = 5, [FromQuery] string? search = null, [FromQuery] int? deptId = null, [FromQuery] string? orderBy = null)
        {
            var data = await _employeeRepo.GetAllEmp(search, deptId, orderBy);
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

        [Authorize(Roles = "Admin")]
        [HttpPut("editEmp/{id}")]
        public async Task<IActionResult> editEmp(int id, RegisterDTO registerDTO)
        {
            var token = Request.Headers.Authorization.ToString().Split(" ")[1];
            var res = await _employeeRepo.editEmp(token, id, registerDTO);
            if (res)
            {
                return Ok(new { msg = "Employee Edited!" });
            }
            return BadRequest(new { msg = "Something went Wrong!!" });
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("deleteEmp/{id}")]
        public async Task<IActionResult> deleteEmployee(int id)
        {
            var token = Request.Headers.Authorization.ToString().Split(" ")[1];
            var res = await _employeeRepo.deleteEmp(token, id);
            if (res)
            {
                return Ok(new { msg = "Employee Deleted!" });
            }
            return BadRequest(new { msg = "Something went Wrong!!!" });
        }
    }
}
