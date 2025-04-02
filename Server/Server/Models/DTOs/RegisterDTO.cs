using System.ComponentModel.DataAnnotations;

namespace Server.Models.DTOs
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "Name is Required")]
        public string EmpName { get; set; }
        [Required(ErrorMessage = "Username is Required")]
        public string UserName { get; set; }
        [EmailAddress(ErrorMessage = "Email is required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string PasswordHash { get; set; }
        [Required(ErrorMessage = "Department is required")]
        public int DeptId { get; set; }
        [Required(ErrorMessage = "Role is required")]
        public int RoleId { get; set; }
        public int? ManagerId { get; set; }
    }
}
