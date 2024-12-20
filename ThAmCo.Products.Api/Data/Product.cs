namespace ThAmCo.Products.Api.Data
{
    public class Product
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public int BrandId { get; set; }
        public Brand? Brand { get; set; }
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        public bool InStock { get; set; }
        public double Price { get; set; }
    }
}
