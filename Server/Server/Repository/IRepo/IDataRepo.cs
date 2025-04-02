using Server.Models.DTOs;

namespace Server.Repository.IRepo
{
    public interface IDataRepo
    {
        Task<List<DepartmentDTO>> getDepartment();
        Task<List<RoleDTO>> getRole();
        Task<List<managerListDTO>> getAllManager();
        Task<List<CriteriaListDTO>> getAllCriteria();
        Task<DashboardData> getDashboardData(string token);
        Task<bool> AddCriteria(string token, CriteriaDTO creteria);
        Task<bool> AddStatus(string token, StatusDTO status);
    }
}
