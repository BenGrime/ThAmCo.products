using Xunit;
using ThAmCo.Products.Api.Controllers;
using ThAmCo.Products.Api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

public class ProductsControllerTests : IDisposable
{
    private readonly DbContextOptions<ProductsDbContext> _dbContextOptions;
    private readonly ProductsDbContext _context;

    public ProductsControllerTests()
    {
        _dbContextOptions = new DbContextOptionsBuilder<ProductsDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
        _context = new ProductsDbContext(_dbContextOptions);
    }

    private async Task SeedDatabase()
    {
        // Clear the database before seeding
        _context.Products.RemoveRange(_context.Products);
        await _context.SaveChangesAsync();

        // Seed the database with initial data
        var products = new List<Product>
        {
            new Product
            {
                Id = 1,
                Name = "Test Product 1",
                Description = "Test Description 1",
                BrandName = "Test Brand 1",
                BrandDescription = "Test Brand Description 1",
                CategoryName = "Test Category 1",
                CategoryDescription = "Test Category Description 1",
                InStock = true,
                Price = 9.99
            },
            new Product
            {
                Id = 2,
                Name = "Test Product 2",
                Description = "Test Description 2",
                BrandName = "Test Brand 2",
                BrandDescription = "Test Brand Description 2",
                CategoryName = "Test Category 2",
                CategoryDescription = "Test Category Description 2",
                InStock = true,
                Price = 19.99
            }
        };

        _context.Products.AddRange(products);
        await _context.SaveChangesAsync();
    }

    [Fact]
    public async Task TestPostProduct()
    {
        // Clear the database before adding a new product
        _context.Products.RemoveRange(_context.Products);
        await _context.SaveChangesAsync();

        var controller = new ProductsController(_context);
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

        var savedProduct = await _context.Products.FindAsync(1);
        Assert.NotNull(savedProduct);
    }

    [Fact]
    public async Task TestGetProduct()
    {
        await SeedDatabase();

        var controller = new ProductsController(_context);
        var result = await controller.GetProductById(1);

        var okResult = Assert.IsType<ActionResult<Product>>(result);
        var product = Assert.IsType<Product>(okResult.Value);
        Assert.Equal(1, product.Id);
    }

    [Fact]
    public async Task TestGetAllProducts()
    {
        await SeedDatabase();

        var controller = new ProductsController(_context);
        var result = await controller.GetProducts();

        var okResult = Assert.IsType<ActionResult<IEnumerable<Product>>>(result);
        var products = Assert.IsType<List<Product>>(okResult.Value);
        Assert.Equal(2, products.Count);
    }

    [Fact]
    public async Task TestGetProductNotFound()
    {
        var controller = new ProductsController(_context);
        var result = await controller.GetProductById(999);

        var notFoundResult = Assert.IsType<ActionResult<Product>>(result);
        Assert.IsType<NotFoundResult>(notFoundResult.Result);
    }

    public void Dispose()
    {
        // Clean up the database after each test
        _context.Products.RemoveRange(_context.Products);
        _context.SaveChanges();
        _context.Dispose();
    }
}