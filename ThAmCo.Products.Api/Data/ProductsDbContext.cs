using Microsoft.EntityFrameworkCore;
using ThAmCo.Products.Api.Data;

namespace ThAmCo.Products.Api.Data
{
    public class ProductsDbContext : DbContext
    {
        private string DbPath { get; set; } = string.Empty;
        public ProductsDbContext(DbContextOptions<ProductsDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            // Define relationships if needed

            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Name = "Goblin Serum",
                    Description = "An experimental serum enhancing strength, agility, and intellect. Handle with caution due to potential side effects.",
                    BrandName = "Oscorp Industries",
                    BrandDescription = "Pioneering advancements in biotechnology, engineering, and genetic research.",
                    CategoryName = "Biotech Enhancements",
                    CategoryDescription = "Cutting-edge innovations designed to augment human capabilities.",
                    InStock = true,
                    Price = 199.99
                },
                new Product
                {
                    Id = 2,
                    Name = "Nano suit",
                    Description = "A Full suit of armour made of nanotechnology. Provides enhanced protection and strength.",
                    BrandName = "Stark Industries",
                    BrandDescription = "To put a suit of armouny around the world.",
                    CategoryName = "Nano Technology",
                    CategoryDescription = "Innovative technologies manipulating matter at the atomic scale for advanced applications.",
                    InStock = true,
                    Price = 99999.99
                }
            );
        }

        public DbSet<Product> Products { get; set; } 
    }
}