using AutoMapper;
using Mangoes.Services.ProductAPI.Model;

namespace Mangoes.Services.ProductAPI
{
    public class MappingConfig 
    {
        public static MapperConfiguration Register()
        {
            var mapperConfig = new MapperConfiguration(config=>
            {
                config.CreateMap<Product, ProductDTO>();
                config.CreateMap<ProductDTO, Product>();

            });
            return mapperConfig;
        }
    }
}
