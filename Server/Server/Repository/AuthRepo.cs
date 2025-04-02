using Microsoft.EntityFrameworkCore;
using Server.Models;
using Server.Models.DTOs;
using Server.Repository.IRepo;
using Server.Services;
using System.Net.WebSockets;

namespace Server.Repository
{
    public class AuthRepo : IAuthRepo
    {
        private readonly AppDbContext _context;
        private readonly TokenService _tokenService;
        private readonly PasswordService _passwordService;
        private readonly SMTPService _smtpService;
        public AuthRepo(AppDbContext context, TokenService tokenService, PasswordService passwordService, SMTPService smtpService)
        {
            _context = context;
            _tokenService = tokenService;
            _passwordService = passwordService;
            _smtpService = smtpService;
        }
        public async Task<string?> Login(LoginDTO loginDTO)
        {
            var user = await _context.Employees.Include(a => a.Role).FirstOrDefaultAsync(a => a.UserName == loginDTO.UserName);

            if (user == null || !(_passwordService.VerifyPassword(loginDTO.PasswordHash, user.PasswordHash)))
            {
                return string.Empty;
            }
            return _tokenService.GenerateToken(user, user.Role.RoleName);
        }

        public async Task<bool> Register(string token, RegisterDTO registerDTO)
        {
            try
            {
                var list = _tokenService.GetData(token);
                var id = list.FirstOrDefault(e => e.Type == "id")?.Value;
                var emp = new Employee
                {
                    DeptId = registerDTO.DeptId,
                    Email = registerDTO.Email,
                    EmpName = registerDTO.EmpName,
                    UserName = registerDTO.UserName,
                    RoleId = registerDTO.RoleId,
                    CreatedBy = Convert.ToInt32(id)
                };
                if (registerDTO.ManagerId != null)
                {
                    emp.ManagerId = registerDTO.ManagerId;
                }
                emp.PasswordHash = _passwordService.HashPassword(registerDTO.PasswordHash);
                await _context.Employees.AddAsync(emp);
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

                await _smtpService.SendEmailAsync(registerDTO.Email, "Welcome to PMS - Sign in Details", body);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
