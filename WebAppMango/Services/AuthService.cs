using WebAppMango.Models.DTO;
using static WebAppMango.Utilities.SD;

namespace WebAppMango.Services
{
    public class AuthService : IAuthService
    {
        private readonly IBusService _busService;
        public AuthService(IBusService busService)
        {
            _busService = busService;
        }

        public async Task<ResponseDTO> AssignRole(RegisterRequestDTO registerRequestDTO)
        {
            return await _busService.SendAsync(new RequestDTO
            {
                APIType = APITypes.POST,
                Data = registerRequestDTO,
                Url = AuthAPIUrl + "/api/auth/Assignrole"
            });
        }

        public async Task<ResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            return await _busService.SendAsync(new RequestDTO
            {
                APIType = APITypes.POST,
                Data = loginRequestDTO,
                Url = AuthAPIUrl + "/api/auth/login"
            }, withBearer: false);
        }

        public async Task<ResponseDTO> Registration(RegisterRequestDTO registerRequestDTO)
        {
            return await _busService.SendAsync(new RequestDTO 
            {
                APIType = APITypes.POST, 
                Data = registerRequestDTO,
                Url = AuthAPIUrl + "/api/auth/register"
            }, withBearer: false);

        }
    }
}
