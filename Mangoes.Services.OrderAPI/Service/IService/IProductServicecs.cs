using Mangoes.Services.OrderAPI.Model.DTO;

namespace Mangoes.Services.OrderAPI.Service.IService
{
    public interface IProductServicecs
    {
        Task<IEnumerable<ProductDTO>> GetProductsAsync();
    }
}
