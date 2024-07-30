using WebAppMango.Models.DTO;

namespace WebAppMango.Services
{
    public interface ICartService
    {
        Task<ResponseDTO> UpsertCart(CartDTO cartDTO);
        Task<ResponseDTO> RemoveCart(int cartId);
        Task<ResponseDTO> ApplyCoupon(CartDTO cartDTO);
        Task<ResponseDTO> RemoveCoupon(CartDTO cartDTO);
        Task<ResponseDTO> GetCart(string userId);
        Task<ResponseDTO> EmailCart(CartDTO cartDTO);
    }
}
