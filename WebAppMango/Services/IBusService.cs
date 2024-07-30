using WebAppMango.Models.DTO;

namespace WebAppMango.Services
{
    public interface IBusService
    {
       Task<ResponseDTO> SendAsync(RequestDTO requestDTO, bool withBearer=true);
    }
}
