using Mangoes.Services.CartAPI.Model.DTO;

namespace Mangoes.Services.CartAPI.Service.IService
{
    public interface ICouponService
    {
        public Task<CouponDTO> GetCoupon(string couponId);
    }
}
