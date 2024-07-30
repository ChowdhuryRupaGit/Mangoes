using WebAppMango.Models.DTO;
using static WebAppMango.Utilities.SD;

namespace WebAppMango.Services
{
    public class OrderService : IOrderService
    {
        public readonly IBusService _service;
        public OrderService(IBusService service)
        {
            _service = service;
        }


        public async Task<ResponseDTO> CreateOrder(CartDTO cartDTO)
        {
            return await _service.SendAsync(new RequestDTO()
            {
                APIType = APITypes.POST,
                Data = cartDTO,
                Url = OrderAPIUrl + "/api/order/CreateOrders"
            });
        }

        public async Task<ResponseDTO> CreateSession(StripeRequestDTO stripeRequestDTO)
        {
            return await _service.SendAsync(new RequestDTO()
            {
                APIType = APITypes.POST,
                Data = stripeRequestDTO,
                Url = OrderAPIUrl + "/api/order/CreateStripeSession"
            });
        }
          public async Task<ResponseDTO> ValidateStripeSession(int orderId)
            {
                return await _service.SendAsync(new RequestDTO()
                {
                    APIType = APITypes.POST,
                    Data = orderId,
                    Url = OrderAPIUrl + "/api/order/ValidateStripeSession"
                });
            }

    }
}
