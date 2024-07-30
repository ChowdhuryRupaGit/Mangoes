using WebAppMango.Models.DTO;
using static WebAppMango.Utilities.SD;

namespace WebAppMango.Services
{
    public class CouponService : ICouponService
    {
        public readonly IBusService _service;
        public CouponService(IBusService service)
        {
            _service = service;
        }
        public async Task<ResponseDTO> AddAsync(CouponDTO coupondto)
        {
            return await _service.SendAsync(new RequestDTO
            {
                APIType = APITypes.POST,
                Data = coupondto,
                Url = CouponBaseUrl + "/api/coupon"
            });
        }

        public async Task<ResponseDTO?> GetAllCouponAsync()
        {
            return await _service.SendAsync(new RequestDTO
            {
                APIType = APITypes.GET,
                Url = CouponBaseUrl + "/api/coupon"
            }) ;

        }

        public async Task<ResponseDTO> GetByCodeAsync(string couponCode)
        {
            return await _service.SendAsync(new RequestDTO
            {
                APIType = APITypes.GET,
                Url = CouponBaseUrl + "/api/coupon/GetByCode/" + couponCode
            });
        }
              
        public async Task<ResponseDTO> GetByIdAsync(int id)
        {
            return await _service.SendAsync(new RequestDTO
            {
                APIType = APITypes.GET,
                Url = CouponBaseUrl + "/api/coupon/GetById/" + id
            });
        }

        public async Task<ResponseDTO> RemoveAsync(int id)
        {
            return await _service.SendAsync(new RequestDTO
            {
                APIType = APITypes.DELETE,
                Url = CouponBaseUrl + "/api/coupon/" + id
            });
        }

        public async Task<ResponseDTO> UpdateAsync(CouponDTO coupondto)
        {
            return await _service.SendAsync(new RequestDTO
            {
                APIType = APITypes.PUT,
                Url = CouponBaseUrl + "/api/coupon/Update",
                Data= coupondto
            });
        }
    }
}
