using Server.Models.DTOs;

namespace Server.Repository.IRepo
{
    public interface IAuthRepo
    {
        Task<string?> Login(LoginDTO loginDTO);
        Task<bool> Register(string token,RegisterDTO registerDTO);
    }
}
