using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Server.Models.DTOs;
using Server.Repository.IRepo;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        private readonly IDataRepo _dataRepo;
        private readonly IMemoryCache _cache;
        public DataController(IDataRepo dataRepo, IMemoryCache cache)
        {
            _dataRepo = dataRepo;
            _cache = cache;
        }

        [Authorize]
        [HttpGet("dashboardData")]
        public async Task<IActionResult> getDashboardData()
        {
            var token = Request.Headers.Authorization.ToString().Split(" ")[1];
            var res = await _dataRepo.getDashboardData(token);

            return Ok(res);
        }

        [Authorize]
        [HttpGet("getAllDept")]
        public async Task<IActionResult> getAllDept()
        {
            if (_cache.TryGetValue("DepartmentCached", out var cachedData))
            {
                return Ok(cachedData);
            }
            var res = await _dataRepo.getDepartment();

            _cache.Set("DepartmentCached", res, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
            });

            return Ok(res);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("getAllRole")]
        public async Task<IActionResult> getAllRole()
        {
            if (_cache.TryGetValue("RoleCached", out var cachedData))
            {
                return Ok(cachedData);
            }
            var res = await _dataRepo.getRole();

            _cache.Set("RoleCached", res, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
            });
            return Ok(res);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("getAllManager")]
        public async Task<IActionResult> getAllManager()
        {
            if (_cache.TryGetValue("ManagerCached", out var cachedData))
            {
                return Ok(cachedData);
            }
            var res = await _dataRepo.getAllManager();
            _cache.Set("ManagerCached", res, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
            });
            return Ok(res);
        }

        [Authorize]
        [HttpGet("getAllCriteria")]
        public async Task<IActionResult> getAllCriteria()
        {
            if (_cache.TryGetValue("CriteriaCached", out var cachedData))
            {
                return Ok(cachedData);
            }
            var res = await _dataRepo.getAllCriteria();
            _cache.Set("CriteriaCached", res, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
            });
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
