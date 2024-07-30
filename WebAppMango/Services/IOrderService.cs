using WebAppMango.Models.DTO;

namespace WebAppMango.Services
{
    public interface IOrderService
    {
       Task<ResponseDTO> CreateOrder(CartDTO cartDTO);
        Task<ResponseDTO> CreateSession(StripeRequestDTO stripeRequestDTO);
        Task<ResponseDTO> ValidateStripeSession(int orderId);
    }
}
