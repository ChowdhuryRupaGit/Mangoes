using WebAppMango.Models.DTO;
using static WebAppMango.Utilities.SD;

namespace WebAppMango.Services
{
    public class ProductService : IProductService
    {
        public readonly IBusService _service;
        public ProductService(IBusService service)
        {
            _service = service;
        }
        public async Task<ResponseDTO> AddAsync(ProductDTO productdto)
        {
            return await _service.SendAsync(new RequestDTO
            {
                APIType = APITypes.POST,
                Data = productdto,
                Url = ProductAPIUrl + "/api/product"
            });
        }

        public async Task<ResponseDTO?> GetAllProductAsync()
        {
            return await _service.SendAsync(new RequestDTO
            {
                APIType = APITypes.GET,
                Url = ProductAPIUrl + "/api/product"
            }) ;

        }

        public async Task<ResponseDTO> GetByIdAsync(int productId)
        {
            return await _service.SendAsync(new RequestDTO
            {
                APIType = APITypes.GET,
                Url = ProductAPIUrl + "/api/product/GetById/" + productId
            });
        }

        public async Task<ResponseDTO> GetByNameAsync(string productName)
        {
            return await _service.SendAsync(new RequestDTO
            {
                APIType = APITypes.GET,
                Url = ProductAPIUrl + "/api/product/GetByName/" + productName
            });
        }
              

        public async Task<ResponseDTO> RemoveAsync(int id)
        {
            return await _service.SendAsync(new RequestDTO
            {
                APIType = APITypes.DELETE,
                Url = ProductAPIUrl + "/api/product/" + id
            });
        }

        public async Task<ResponseDTO> UpdateAsync(ProductDTO productdto)
        {
            return await _service.SendAsync(new RequestDTO
            {
                APIType = APITypes.PUT,
                Url = ProductAPIUrl + "/api/product/",
                Data= productdto
            });
        }
    }
}
