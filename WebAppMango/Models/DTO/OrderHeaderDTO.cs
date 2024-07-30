namespace WebAppMango.Models.DTO
{
    public class OrderHeaderDTO
    {
        public int OrderHeaderId { get; set; }
        public string? UserId { get; set; }
        public double TotalPrice { get; set; }
        public string? CouponCode { get; set; }
        public string? Name { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public DateTime OrderTime { get; set; }
        public double Discount { get; set; }
        public string? StatusCode { get; set; }
        public string? PaymentIntentId { get; set; }
        public string? StripeSessionId { get; set; }
        public IEnumerable<OrderDetailsDTO> OrderDetailsList { get; set; }
    }
}
