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
    public async Task TestGetProductById()
    {
        // Step 1: Load the database with products
        await SeedDatabase();

        // Step 2: Test getting one of them by ID
        var controller = new ProductsController(_context);
        var result = await controller.GetProductById(1);

        // Check if the product ID retrieved matches the input
        var okResult = Assert.IsType<ActionResult<Product>>(result);
        var product = Assert.IsType<Product>(okResult.Value);
        Assert.Equal(1, product.Id);
    }

    [Fact]
    public async Task TestGetAllProducts()
    {
        // Step 1: Load the database with products
        await SeedDatabase();

        // Step 3: Test getting all of them
        var controller = new ProductsController(_context);
        var result = await controller.GetProducts();

        // Check if the list length returned equals the amount in the database
        var okResult = Assert.IsType<ActionResult<IEnumerable<Product>>>(result);
        var products = Assert.IsType<List<Product>>(okResult.Value);
        Assert.Equal(2, products.Count);
    }

    public void Dispose()
    {
        // Clean up the database after each test
        _context.Products.RemoveRange(_context.Products);
        _context.SaveChanges();
        _context.Dispose();
    }
}