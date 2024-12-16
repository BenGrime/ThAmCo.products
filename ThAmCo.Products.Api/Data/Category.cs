namespace ThAmCo.Products.Api.Data
{
    public class Category
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public int AvailableProductCount { get; set; }
    }
}
