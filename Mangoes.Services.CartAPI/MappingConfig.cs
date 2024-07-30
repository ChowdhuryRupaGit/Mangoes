using AutoMapper;
using Mangoes.Services.CartAPI.Model;
using Mangoes.Services.CartAPI.Model.DTO;

namespace Mangoes.Services.CartAPI
{
    public class MappingConfig 
    {
        public static MapperConfiguration Register()
        {
            var mapperConfig = new MapperConfiguration(config=>
            {
                config.CreateMap<CartHeader, CartHeaderDTO>().ReverseMap();
                config.CreateMap<CartDetails, CartDetailsDTO>().ReverseMap();
            });
            return mapperConfig;
        }
    }
}
