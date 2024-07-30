using WebAppMango.Models.DTO;

namespace WebAppMango.Services
{
    public interface IAuthService
    {
        Task<ResponseDTO> Registration(RegisterRequestDTO registerRequestDTO);
        Task<ResponseDTO> Login(LoginRequestDTO loginRequestDTO);
        Task<ResponseDTO> AssignRole(RegisterRequestDTO registerRequestDTO);
    }
}
