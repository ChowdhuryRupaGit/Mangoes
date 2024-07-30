﻿
using System.ComponentModel.DataAnnotations;

namespace WebAppMango.Models.DTO
{
    public class CartHeaderDTO
    {
        public int CartHeaderId { get; set; }
        public string UserId { get; set; }
        public double TotalPrice { get; set; }
        public string? CouponCode { get; set; }
        public double Discount { get; set; }

        [Required]
        public string? Name { get; set;}

        [Required]
        public string? PhoneNumber { get; set; }

        [Required]
        public string? EmailId { get; set; }
    }
}
