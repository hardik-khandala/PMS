using Microsoft.EntityFrameworkCore;

namespace Server.Models.DTOs
{
    [Keyless]
    public class EmployeeDTO
    {
        public int EmpId { get; set; }
        public string EmpName { get; set; }
        public string UserName { get; set; } 
        public string Email { get; set; }
        public string Department { get; set; }
        public string Role { get; set; }
        public DateTime? JoiningDate { get; set; }
    }
}
