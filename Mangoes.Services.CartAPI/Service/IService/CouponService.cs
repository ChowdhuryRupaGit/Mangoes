using Mangoes.Services.CartAPI.Model.DTO;
using Newtonsoft.Json;

namespace Mangoes.Services.CartAPI.Service.IService
{
    public class CouponService : ICouponService
    {
        IHttpClientFactory _httpClientFactory;
        public CouponService(IHttpClientFactory httpClientFactory)
        {

            _httpClientFactory = httpClientFactory;

        }
        public async Task<CouponDTO> GetCoupon(string couponCode)
        {
           var client = _httpClientFactory.CreateClient("Coupon");
            var response  = await client.GetAsync("/api/coupon/GetByCode/" + couponCode);
            var content = await response.Content.ReadAsStringAsync();
            var apiResonseDTO = JsonConvert.DeserializeObject<ResponseDTO>(content);
            if(apiResonseDTO.IsSuccess)
            {
                return JsonConvert.DeserializeObject<CouponDTO>(Convert.ToString(apiResonseDTO.Result));
            }
            else return new CouponDTO();
        }
    }
}
