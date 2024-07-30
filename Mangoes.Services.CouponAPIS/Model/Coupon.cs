using System.ComponentModel.DataAnnotations;

namespace Mangoes.Services.CouponAPIS.Model
{
    public class Coupon
    {
        [Key]
        public int CouponId { get; set; }

        [Required]
        public string CouponCode { get; set; }

        [Required]
        public double DiscountAmount { get; set; }

        [Required]
        public int MinAmount { get; set; }

    }
}
