using Microsoft.EntityFrameworkCore;
using Server.Models;
using Server.Models.DTOs;
using Server.Repository.IRepo;
using Server.Services;

namespace Server.Repository
{
    public class EmployeeRepo : IEmployeeRepo
    {
        private readonly AppDbContext _context;
        private readonly TokenService _tokenService;
        private readonly PasswordService _passwordService;
        private readonly SMTPService _smtpService;
        public EmployeeRepo(AppDbContext context, TokenService tokenService, PasswordService passwordService, SMTPService smtpService)
        {
            _context = context;
            _tokenService = tokenService;
            _passwordService = passwordService;
            _smtpService = smtpService;
        }

        public async Task<RegisterDTO> GetEmp(int empId)
        {
            var emp = await _context.Employees.FirstOrDefaultAsync(e => e.EmpId == empId);
            var res = new RegisterDTO
            {
                UserName = emp.UserName,
                Email = emp.Email,
                DeptId = emp.DeptId,
                EmpName = emp.EmpName,
                ManagerId = emp.ManagerId,
                RoleId = emp.RoleId,
            };

            return res;
        }

        public async Task<List<EmployeeDTO>> GetAllEmp()
        {
            var data = await _context.Employees.Include(e => e.Dept).Include(e => e.Role).Where(e => e.IsDeleted == false)
                .Select(e => new EmployeeDTO
                {
                    UserName = e.UserName,
                    Department = e.Dept.DeptName,
                    Email = e.Email,
                    EmpId = e.EmpId,
                    EmpName = e.EmpName,
                    JoiningDate = e.JoiningDate,
                    Role = e.Role.RoleName
                }).ToListAsync();
            return data;
        }

        public async Task<bool> editEmp(string token, int empId, RegisterDTO registerDTO)
        {
            try
            {
                var list = _tokenService.GetData(token);
                var id = list.FirstOrDefault(e => e.Type == "id")?.Value;
                var emp = await _context.Employees.FirstOrDefaultAsync(e => e.EmpId == empId);
                if (emp == null)
                {
                    return false;
                }
                emp.DeptId = registerDTO.DeptId;
                emp.UserName = registerDTO.UserName;
                emp.RoleId = registerDTO.RoleId;
                emp.EmpName = registerDTO.EmpName;
                emp.Email = registerDTO.Email;
                emp.ManagerId = registerDTO.ManagerId;
                emp.PasswordHash = _passwordService.HashPassword(registerDTO.PasswordHash);
                emp.ModifyAt = DateOnly.FromDateTime(DateTime.UtcNow);
                emp.ModiftyBy = Convert.ToInt32(id);

                await _context.SaveChangesAsync();

                string body = $@"
                <html>
                <head>
                <style>
                    p{{
                        color: red;
                    }}
                </style>
                </head>
                <body>
                    <h2>Welcome {registerDTO.EmpName}</h2>
 
                    <p>Note: Do not share your credetials with anyone!!</p> 

                    <h3>Username: {registerDTO.UserName}</h3>
                    <h3>Password: {registerDTO.PasswordHash}</h3>

                </body>
                </html>";
                await _smtpService.SendEmailAsync(registerDTO.Email, "Your Details has been Changed", body);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> deleteEmp(string token, int empId)
        {
            try
            {
                var list = _tokenService.GetData(token);
                var id = list.FirstOrDefault(e => e.Type == "id")?.Value;

                var emp = await _context.Employees.FirstOrDefaultAsync(e => e.EmpId == empId);
                if (emp is null)
                {
                    return false;
                }
                emp.IsDeleted = true;
                emp.ModiftyBy = Convert.ToInt32(id);
                emp.ModifyAt = DateOnly.FromDateTime(DateTime.Now);
                await _context.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}
