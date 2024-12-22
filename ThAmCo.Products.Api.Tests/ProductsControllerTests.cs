using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using ThAmCo.Products.Api;
using ThAmCo.Products.Api.Data;
using ThAmCo.Products.Api.Controllers;
using Xunit;
using Microsoft.AspNetCore.Mvc;

namespace ThAmCo.Products.Api.Tests
{
    public class ProductsControllerTests
    {
        private DbContextOptions<ProductsDbContext> _dbContextOptions;

        public ProductsControllerTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<ProductsDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
        }

        [Fact]
        public async Task TestPostProduct()
        {
            using (var context = new ProductsDbContext(_dbContextOptions))
            {
                var controller = new ProductsController(context);
                var dto = new ProductDto
                {
                    Name = "Test Product",
                    Id = 1,
                    Description = "Test Description",
                    BrandName = "Test Brand",
                    BrandDescription = "Test Brand Description",
                    CategoryName = "Test Category",
                    CategoryDescription = "Test Category Description",
                    InStock = true,
                    Price = 9.99
                };

                var result = await controller.PostProduct(dto);
                Assert.IsType<OkResult>(result);

                var savedProduct = await context.Products.FindAsync(1);
                Assert.NotNull(savedProduct);
                Assert.Equal("Test Product", savedProduct.Name);
            }
        }
    }
}