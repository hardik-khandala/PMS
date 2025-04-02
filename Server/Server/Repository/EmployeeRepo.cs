using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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

        public async Task<List<EmployeeDTO>> GetAllEmp(string? search, int? deptId, string? orderBy)
        {
            var data = _context.Employees.Include(e => e.Dept).Include(e => e.Role).Where(e => e.IsDeleted == false);

            if (deptId.HasValue && deptId > 0)
            {
                data = data.Where(e => e.DeptId == deptId);
            };

            if (!search.IsNullOrEmpty())
            {
                data = data.Where(e => e.EmpName.ToLower().Contains(search.ToLower()));
            }
            if (!orderBy.IsNullOrEmpty())
            {
                if (orderBy == "asc")
                {
                    data = data.OrderBy(e => e.EmpName);
                }
                else if(orderBy == "dsc")
                {
                    data = data.OrderByDescending(e => e.EmpName);
                }
            }


            var res = await data
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
            return res;


            //var parameters = new[]
            //{
            //    new SqlParameter("@Search", search ?? (object)DBNull.Value),
            //    new SqlParameter("@DeptId", deptId ?? (object)DBNull.Value),
            //    new SqlParameter("@PageNumber", 1),
            //    new SqlParameter("@PageSize", 5)
            //};

            //var employees = await _context.EmpSP
            //    //.FromSqlRaw("EXEC GetEmployeeData")
            //    .FromSqlRaw("EXEC GetEmployeeData @Search, @DeptId, @PageNumber, @PageSize", parameters)

            //    .ToListAsync();

            //return employees;
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
                emp.ModifyAt = DateOnly.FromDateTime(DateTime.Now);
                emp.ModiftyBy = Convert.ToInt32(id);

                await _context.SaveChangesAsync();

                string body = $@"
<html>
<head>
    <style>
        body {{
            font-family: Arial, sans-serif;
            background-color: #f4f4f4;
            color: #333;
            padding: 20px;
        }}
        h2 {{
            color: #007bff;
        }}
        h3 {{
            color: #555;
        }}
        p {{
            color: #e74c3c;
            font-weight: bold;
        }}
        .footer {{
            margin-top: 20px;
            font-size: 12px;
            color: #999;
        }}
        .credentials {{
            background-color: #fff;
            padding: 15px;
            border-radius: 8px;
            box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1);
            margin-top: 15px;
        }}
        .note {{
            background-color: #f8d7da;
            padding: 10px;
            border-radius: 5px;
            margin-bottom: 20px;
            color: #721c24;
        }}
    </style>
</head>
<body>
    <h2>Welcome, {registerDTO.EmpName}!</h2>
    
    <div class='note'>
        <p>Note: Do not share your credentials with anyone! Keep them secure.</p>
    </div>
    
    <div class='credentials'>
        <h3>Account Details:</h3>
        <h4><strong>Username:</strong> {registerDTO.UserName}</h4>
        <h4><strong>Password:</strong> {registerDTO.PasswordHash}</h4>
    </div>

    <div class='footer'>
        <p>Thank you for registering with us. If you have any questions, feel free to reach out!</p>
        <p>&copy; {DateTime.Now.Year} PMS</p>
    </div>
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
