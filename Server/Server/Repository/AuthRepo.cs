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
