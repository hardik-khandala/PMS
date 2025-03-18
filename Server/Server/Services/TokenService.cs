using Microsoft.IdentityModel.Tokens;
using Server.Models;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Server.Services
{
    public class TokenService
    {
        private readonly IConfiguration _config;
        public TokenService(IConfiguration config)
        {
            _config = config;
        }
        public string? GenerateToken(Employee user, string role)
        {
            List<Claim> claims = new List<Claim>
            {
               new("id",user.EmpId.ToString()),
               new("username", user.UserName),
               new(ClaimTypes.Email,user.Email),
               new(ClaimTypes.Role, role),
            };
           
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims, 
                issuer: _config["Jwt:Issuer"], 
                audience: _config["Jwt:Audience"], 
                expires: DateTime.Now.AddDays(1), 
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public List<Claim> GetData(string token)
        {
            var data = new JwtSecurityToken(token);
            return data.Claims.ToList();
        }
    }
}
