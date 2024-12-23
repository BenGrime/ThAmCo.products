using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using ThAmCo.Products.Api.Data;
using ThAmCo.Products.Api.Controllers;
using Xunit;
using Microsoft.AspNetCore.Mvc;

namespace ThAmCo.Products.Api.Tests
{
    public class DbContextTests
    {
        private DbContextOptions<ProductsDbContext> _dbContextOptions;

        public DbContextTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<ProductsDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
        }

        private async Task SeedDatabase(ProductsDbContext context)
        {
            // Clear the database before seeding
            context.Products.RemoveRange(context.Products);
            await context.SaveChangesAsync();

            var product = new Product
            {
                Name = "Test Product",
                Id = 100,
                Description = "Test Description",
                BrandName = "Test Brand",
                BrandDescription = "Test Brand Description",
                CategoryName = "Test Category",
                CategoryDescription = "Test Category Description",
                InStock = true,
                Price = 9.99
            };

            await context.Products.AddAsync(product);
            await context.SaveChangesAsync();
        }

        private void PrintDatabaseContents(ProductsDbContext context)
        {
            var products = context.Products.ToList();
            foreach (var product in products)
            {
                System.Console.WriteLine($"Id: {product.Id}, Name: {product.Name}, Description: {product.Description}, Price: {product.Price}");
            }
        }

        [Fact]
        public async Task TestAddProduct()
        {
            using (var context = new ProductsDbContext(_dbContextOptions))
            {
                var product = new Product
                {
                    Name = "Test Product",
                    Id = 101, // Use a different ID for this test
                    Description = "Test Description",
                    BrandName = "Test Brand",
                    BrandDescription = "Test Brand Description",
                    CategoryName = "Test Category",
                    CategoryDescription = "Test Category Description",
                    InStock = true,
                    Price = 9.99
                };

                await context.Products.AddAsync(product);
                await context.SaveChangesAsync();

                var savedProduct = await context.Products.FindAsync(101);
                Assert.NotNull(savedProduct);
                Assert.Equal("Test Product", savedProduct.Name);

                // Print the contents of the database
                PrintDatabaseContents(context);
            }
        }

        [Fact]
        public async Task TestGetProductById()
        {
            using (var context = new ProductsDbContext(_dbContextOptions))
            {
                await SeedDatabase(context);

                var savedProduct = await context.Products.FindAsync(100);
                Assert.NotNull(savedProduct);
                Assert.Equal("Test Product", savedProduct.Name);

                // Print the contents of the database
                PrintDatabaseContents(context);
            }
        }

        [Fact]
        public async Task TestUpdateProduct()
        {
            using (var context = new ProductsDbContext(_dbContextOptions))
            {
                await SeedDatabase(context);

                var savedProduct = await context.Products.FindAsync(100);
                Assert.NotNull(savedProduct);

                savedProduct.Name = "Updated Product";
                context.Products.Update(savedProduct);
                await context.SaveChangesAsync();

                var updatedProduct = await context.Products.FindAsync(100);
                Assert.NotNull(updatedProduct);
                Assert.Equal("Updated Product", updatedProduct.Name);

                // Print the contents of the database
                PrintDatabaseContents(context);
            }
        }

        [Fact]
        public async Task TestDeleteProduct()
        {
            using (var context = new ProductsDbContext(_dbContextOptions))
            {
                await SeedDatabase(context);

                var savedProduct = await context.Products.FindAsync(100);
                Assert.NotNull(savedProduct);

                context.Products.Remove(savedProduct);
                await context.SaveChangesAsync();

                var deletedProduct = await context.Products.FindAsync(100);
                Assert.Null(deletedProduct);

                // Print the contents of the database
                PrintDatabaseContents(context);
            }
        }
    }
}