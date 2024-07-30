using Mangoes.Services.CartAPI.Model.DTO;
using Newtonsoft.Json;
using System.Net.Http;

namespace Mangoes.Services.CartAPI.Service.IService
{
    public class ProductServices : IProductServicecs
    {
        public readonly IHttpClientFactory _httpClientFactory;
        public ProductServices(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<IEnumerable<ProductDTO>> GetProductsAsync()
        { 
            HttpClient client = _httpClientFactory.CreateClient("Products");
            var response = await client.GetAsync($"/api/product");
            var apiContent = await response.Content.ReadAsStringAsync();
            var apiResonseDTO = JsonConvert.DeserializeObject<ResponseDTO>(apiContent);
            if(apiResonseDTO.IsSuccess)
            {
                return JsonConvert.DeserializeObject<IEnumerable<ProductDTO>>(Convert.ToString(apiResonseDTO.Result));
            }
            return new List<ProductDTO>();
        }
    }
}
