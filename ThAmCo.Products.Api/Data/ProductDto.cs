using System;

namespace ThAmCo.Products.Api.Data
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public BrandDto Brand { get; set; }
        public CategoryDto Category { get; set; }
        public bool InStock { get; set; }
        public decimal Price { get; set; }
    }
}