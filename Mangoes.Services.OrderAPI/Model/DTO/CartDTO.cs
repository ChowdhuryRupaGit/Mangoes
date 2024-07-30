namespace Mangoes.Services.OrderAPI.Model.DTO
{
    public class CartDTO
    {
        public CartHeaderDTO CartHeaderDTO { get; set; }
        public IEnumerable<CartDetailsDTO> CartDetailsDTOLists { get; set; }
    }
}
