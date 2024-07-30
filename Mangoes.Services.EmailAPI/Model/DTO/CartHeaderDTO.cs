
namespace Mangoes.Services.EmailAPI.Model.DTO
{
    public class CartHeaderDTO
    {
        public int CartHeaderId { get; set; }
        public string UserId { get; set; }
        public double TotalPrice { get; set; }
        public string CouponCode { get; set; }
        public double Discount { get; set; }
        public string? Name { get; set; }
        public string? PhoneNumber { get; set; }
        public string? EmailId { get; set; }
    }
}
