using WebAppMango.Models.DTO;

namespace WebAppMango.Services
{
    public interface ICouponService
    {
        Task<ResponseDTO> GetAllCouponAsync();
        Task<ResponseDTO> GetByIdAsync(int id);
        Task<ResponseDTO> GetByCodeAsync(string couponCode);
        Task<ResponseDTO> AddAsync(CouponDTO coupondto);
        Task<ResponseDTO> UpdateAsync(CouponDTO coupondto);
        Task<ResponseDTO> RemoveAsync(int couponId);

    }
}
