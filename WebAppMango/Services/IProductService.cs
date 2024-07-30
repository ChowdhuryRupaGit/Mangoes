using WebAppMango.Models.DTO;

namespace WebAppMango.Services
{
    public interface IProductService
    {
        Task<ResponseDTO> GetAllProductAsync();
        Task<ResponseDTO> GetByNameAsync(string productName);
        Task<ResponseDTO> AddAsync(ProductDTO productdto);
        Task<ResponseDTO> UpdateAsync(ProductDTO productdto);
        Task<ResponseDTO> RemoveAsync(int productId);
        Task<ResponseDTO> GetByIdAsync(int productId);

    }
}
