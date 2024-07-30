namespace WebAppMango.Models.DTO
{
    public class StripeRequestDTO
    {
        public string ApprovelURL { get; set; }
        public string CancelURL { get; set; }
        public string? StripeSessionUrl { get; set; }
        public string? StripeSessionId { get; set; }
        public OrderHeaderDTO OrderHeaderDTO { get; set; }
    }
}
