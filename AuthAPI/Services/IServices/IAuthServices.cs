using Mangoes.Services.AuthAPI.Model.DTO;

namespace Mangoes.Services.AuthAPI.Services.IServices
{
    public interface IAuthServices
    {
        Task<string> Registration(RegisterRequestDTO registerRequestDTO);
        Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);
        Task<bool> AssignRole(string emailId, string role);
    }
}
