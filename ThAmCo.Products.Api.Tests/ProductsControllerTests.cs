using Xunit;
using ThAmCo.Products.Api.Controllers;
using ThAmCo.Products.Api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

public class ProductsControllerTests
{
    private readonly DbContextOptions<ProductsDbContext> _dbContextOptions;

    public ProductsControllerTests()
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

        // Seed the database with initial data
        var product = new Product
        {
            Id = 1,
            Name = "Test Product",
            Description = "Test Description",
            BrandName = "Test Brand",
            BrandDescription = "Test Brand Description",
            CategoryName = "Test Category",
            CategoryDescription = "Test Category Description",
            InStock = true,
            Price = 9.99
        };

        context.Products.Add(product);
        await context.SaveChangesAsync();
    }

    [Fact]
    public async Task TestPostProduct()
    {
        using (var context = new ProductsDbContext(_dbContextOptions))
        {
            // Clear the database before adding a new product
            context.Products.RemoveRange(context.Products);
            await context.SaveChangesAsync();

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
        }
    }

    [Fact]
    public async Task TestGetProduct()
    {
        using (var context = new ProductsDbContext(_dbContextOptions))
        {
            await SeedDatabase(context);

            var controller = new ProductsController(context);
            var result = await controller.GetProductById(1);

            var okResult = Assert.IsType<ActionResult<Product>>(result);
            var product = Assert.IsType<Product>(okResult.Value);
            Assert.Equal(1, product.Id);
        }
    }

    [Fact]
    public async Task TestGetAllProducts()
    {
        using (var context = new ProductsDbContext(_dbContextOptions))
        {
            await SeedDatabase(context);

            var controller = new ProductsController(context);
            var result = await controller.GetProducts();

            var okResult = Assert.IsType<ActionResult<IEnumerable<Product>>>(result);
            var products = Assert.IsType<List<Product>>(okResult.Value);
            Assert.Single(products);
        }
    }

    [Fact]
    public async Task TestGetProductNotFound()
    {
        using (var context = new ProductsDbContext(_dbContextOptions))
        {
            var controller = new ProductsController(context);
            var result = await controller.GetProductById(999);

            var notFoundResult = Assert.IsType<ActionResult<Product>>(result);
            Assert.IsType<NotFoundResult>(notFoundResult.Result);
        }
    }
}