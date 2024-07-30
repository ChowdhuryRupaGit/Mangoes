using WebAppMango.Models.DTO;
using static WebAppMango.Utilities.SD;

namespace WebAppMango.Services
{
    public class CartService : ICartService
    {
        private readonly IBusService _service;
        public CartService(IBusService busService)
        {
            _service = busService;
        }
        public async Task<ResponseDTO> ApplyCoupon(CartDTO cartDTO)
        {
            return await _service.SendAsync(new RequestDTO
            {
                APIType = APITypes.POST,
                Data = cartDTO,
                Url = CartAPIUrl + "/api/cart/ApplyCoupon"
            });
        }

        public async Task<ResponseDTO> GetCart(string userId)
        {
            return await _service.SendAsync(new RequestDTO()
            {
                APIType = APITypes.GET,
                Url = CartAPIUrl + "/api/cart/GetCart/" +  userId
            });
        }

        public async Task<ResponseDTO> RemoveCart(int cartDetailsId)
        {
            return await _service.SendAsync(new RequestDTO
            {
                APIType = APITypes.POST,
                Data = cartDetailsId,
                Url = CartAPIUrl + "/api/cart/RemoveCart"
            });
        }

        public async Task<ResponseDTO> RemoveCoupon(CartDTO cartDTO)
        {
            return await _service.SendAsync(new RequestDTO
            {
                APIType = APITypes.POST,
                Data = cartDTO,
                Url = CartAPIUrl  + "/api/cart/RemoveCoupon"
            });
        }

        public async Task<ResponseDTO> EmailCart(CartDTO cartDTO)
        {
            return await _service.SendAsync(new RequestDTO
            {
                APIType = APITypes.POST,
                Data = cartDTO,
                Url = CartAPIUrl + "/api/cart/EmailCartRequest"
            });
        }

        public async Task<ResponseDTO> UpsertCart(CartDTO cartDTO)
        {
            return await _service.SendAsync(new RequestDTO
            {
                APIType = APITypes.POST,
                Data = cartDTO,
                Url = CartAPIUrl + "/api/cart/CartUpsert"
            });
        }
    }
}
