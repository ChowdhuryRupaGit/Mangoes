using Mangoes.Services.CartAPI.Model.DTO;

namespace Mangoes.Services.CartAPI.Service.IService
{
    public interface IProductServicecs
    {
        Task<IEnumerable<ProductDTO>> GetProductsAsync();
    }
}
