namespace ThAmCo.Products.Api.Data
{
    public class Product
    {
        public int BrandId { get; set; }

        public string?BrandName { get; set; }

        public int CategoryId { get; set; }

        public string? CategoryName { get; set; }

        public string? Description { get; set; }

        public int Id { get; set; }

        public string? Name { get; set; }

        public Boolean inStock { get; set; }

        public double price { get; set; }
    }
}
