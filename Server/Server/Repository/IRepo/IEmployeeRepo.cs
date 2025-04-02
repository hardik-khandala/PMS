using Server.Models.DTOs;

namespace Server.Repository.IRepo
{
    public interface IEmployeeRepo
    {
        Task<RegisterDTO> GetEmp(int empId);
        Task<List<EmployeeDTO>> GetAllEmp(string? search, int? deptId, string? orderBy);
        Task<bool> editEmp(string token, int empId, RegisterDTO registerDTO);
        Task<bool> deleteEmp(string token, int empId);
    }
}
