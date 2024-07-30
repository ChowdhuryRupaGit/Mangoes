using AutoMapper;
using Mangoes.Services.CouponAPIS.Model;

namespace Mangoes.Services.CouponAPIS
{
    public class MappingConfig 
    {
        public static MapperConfiguration Register()
        {
            var mapperConfig = new MapperConfiguration(config=>
            {
                config.CreateMap<Coupon, CouponDTO>();
                config.CreateMap<CouponDTO, Coupon>();

            });
            return mapperConfig;
        }
    }
}
