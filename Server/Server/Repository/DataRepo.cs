using Microsoft.EntityFrameworkCore;
using Server.Models;
using Server.Models.DTOs;
using Server.Repository.IRepo;
using Server.Services;
using System.Security.AccessControl;
using System.Security.Claims;

namespace Server.Repository
{
    public class DataRepo : IDataRepo
    {
        private readonly AppDbContext _context;
        private readonly TokenService _tokenService;
        public DataRepo(AppDbContext context, TokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public async Task<List<DepartmentDTO>> getDepartment()
        {
            var res = await _context.Departments
                .Select(d => new DepartmentDTO
                {
                    DeptId = d.DeptId,
                    DeptName = d.DeptName
                }).ToListAsync();
            return res;
        }

        public async Task<List<RoleDTO>> getRole()
        {
            var res = await _context.RoleTables
                .Select(d => new RoleDTO
                {
                    RoleId = d.RoleId,
                    RoleName = d.RoleName
                }).ToListAsync();
            return res;
        }

        public async Task<List<managerListDTO>> getAllManager()
        {
            var res = await _context.Employees.Where(e => e.RoleId == 2)
                .Select(e => new managerListDTO
                {
                    EmpId = e.EmpId,
                    EmpName = e.EmpName
                }).ToListAsync();

            return res;
        }

        public async Task<List<CriteriaListDTO>> getAllCriteria()
        {
            var res = await _context.PredefinedCriteria.Where(c => c.IsDeleted == false)
                .Select(c => new CriteriaListDTO
                {
                    CriteriaId = c.CriteriaId,
                    CriteriaName = c.CriteriaName,
                    Details = c.Details,
                }).ToListAsync();

            return res;
        }

        public async Task<bool> AddCriteria(string token, CriteriaDTO creteria)
        {
            try
            {
                var list = _tokenService.GetData(token);
                var id = list.FirstOrDefault(e => e.Type == "id")?.Value;
                var data = new PredefinedCriterion
                {
                    CriteriaName = creteria.CriteriaName,
                    Details = creteria.Details,
                    CreatedBy = Convert.ToInt32(id)
                };
                await _context.AddAsync(data);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> AddStatus(string token, StatusDTO status)
        {
            try
            {
                var list = _tokenService.GetData(token);
                var id = list.FirstOrDefault(e => e.Type == "id")?.Value;
                var data = new StatusTable
                {
                    Status = status.Status,
                    CreatedBy = Convert.ToInt32(id)
                };
                await _context.AddAsync(data);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<DashboardData> getDashboardData(string token)
        {
            var list = _tokenService.GetData(token);
            var id = list.FirstOrDefault(e => e.Type == "id")?.Value;
            var role = list.FirstOrDefault(e => e.Type == ClaimTypes.Role)?.Value;

            var emp = await _context.Employees.Where(e => e.IsDeleted == false).ToListAsync();
            var review = await _context.PerfomanceReviews.Where(e => e.IsDeleted == false).ToListAsync();
            var goal = await _context.Goals.Where(e => e.IsDeleted == false).ToListAsync();
            var report = await _context.PerfomanceReviews.Where(r => r.StatusId == 2 && r.IsDeleted == false).ToListAsync();

            if (role == "Admin" || role == "HR")
            {
                var response = new DashboardData
                {
                    TotalEmployee = emp.Count(),
                    TotalReview = review.Count(),
                    TotalGoal = goal.Count(),
                    TotalReport = report.Count()
                };
                return response;
            }
            else
            {
                var response = new DashboardData
                {
                    TotalEmployee = emp.Count(),
                    TotalReview = review.Where(r=>r.EmpId == Convert.ToInt32(id)).Count(),
                    TotalGoal = goal.Where(r => r.EmpId == Convert.ToInt32(id)).Count(),
                };
                return response;
            }
        }
    }
}
