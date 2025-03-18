namespace Server.Models.DTOs
{
    public class RegisterDTO
    {
        public string EmpName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; } 
        public int DeptId { get; set; }
        public int RoleId { get; set; }
        public int? ManagerId { get; set; }
    }
}
