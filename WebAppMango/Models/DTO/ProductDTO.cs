﻿
using System.ComponentModel.DataAnnotations;

namespace WebAppMango.Models.DTO
{
    public class ProductDTO
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public double Price { get; set; }
        public string Description { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        [Range(1,20)]
        public int Count { get; set; } = 1;
    }
}
