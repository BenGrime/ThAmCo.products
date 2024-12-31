using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Globalization;
using ThAmCo.Products.Api.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["Auth:Authority"];
        options.Audience = builder.Configuration["Auth:Audience"];
    });
builder.Services.AddAuthorization();

builder.Services.AddDbContext<ProductsDbContext>(options =>
{

    if (builder.Environment.IsDevelopment())
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        var dbPath = System.IO.Path.Join(path, "ThAmCo.Products.db");
        options.UseSqlite($"Data Source={dbPath}");
        options.EnableDetailedErrors();
        options.EnableSensitiveDataLogging();
    }
    else
    {
        var cs = builder.Configuration.GetConnectionString("DefaultConnection");
        options.UseSqlServer(cs, sqlServerOptionsAction: sqlOptions =>
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(6),
            errorNumbersToAdd: null
            )
        );
    }
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

var products = new[] {
    new ProductTemp(1, "Smartphone X", "Latest flagship smartphone", "TechCorp", "Leading innovation in electronics", "Electronics", "Devices that enhance daily life through technology", true, 999.99),
    new ProductTemp(2, "Smart TV", "4K UHD Smart TV", "TechCorp", "Pioneering 4K display technology", "Electronics", "High-quality entertainment through cutting-edge tech", true, 799.99),
    new ProductTemp(3, "Smart Fridge", "Energy-efficient smart fridge", "EcoBrands", "Eco-friendly appliances for modern living", "Home Appliances", "Appliances designed for smart, sustainable living", true, 1499.99),
    new ProductTemp(4, "Smart Speaker", "Smart speaker with voice assistant", "EcoBrands", "Combining sound and AI for a smarter home", "Electronics", "Smart devices that improve home convenience", true, 199.99)
};


var responseMessage = app.Configuration["Message"] ?? "";

app.MapGet("/products" , [Authorize] async(ProductsDbContext dbx)  => 
{
    return await dbx.Products.ToListAsync();        
});
// .WithName("GetProducts")
// .WithOpenApi();
app.MapGet("/products/{id}", [Authorize] async(ProductsDbContext dbx, int id) =>
{
    var product = await dbx.Products.FirstOrDefaultAsync(p => p.Id == id);
    if (product == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(product);
    
});

app.MapPost("/products",  async (ProductsDbContext dbx, ProductDto dto) =>
{
    var product = new Product
    {
        Name = dto.Name,
        Description = dto.Description,
        BrandName = dto.BrandName,
        BrandDescription = dto.BrandDescription,
        CategoryName = dto.CategoryName,
        CategoryDescription = dto.CategoryDescription,
        InStock = dto.InStock,
        Price = (double)dto.Price
    };
    await dbx.Products.AddAsync(product);
    await dbx.SaveChangesAsync();
    return responseMessage;
});

app.Run();

record ProductTemp(int Id, string? Name, string? Description, string? BrandName, string? BrandDescription, string? CategoryName, string? CategoryDescription, bool InStock, double Price);