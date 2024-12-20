using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
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
    // var cs = builder.Configuration.GetConnectionString("DefaultConnection");
    // options.UseSqlServer(cs, sqlServerOptionsAction: sqlOptions =>
    //     sqlOptions.EnableRetryOnFailure(
    //         maxRetryCount: 5,
    //         maxRetryDelay: TimeSpan.FromSeconds(6),
    //         errorNumbersToAdd: null
    //     )
    // );

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

// app.MapControllers();

// var summaries = new[]
// {
//     "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
// };

// app.MapGet("/weatherforecast", () =>
// {
//     var forecast =  Enumerable.Range(1, 5).Select(index =>
//         new WeatherForecast
//         (
//             DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
//             Random.Shared.Next(-20, 55),
//             summaries[Random.Shared.Next(summaries.Length)]
//         ))
//         .ToArray();
//     return forecast;
// })
// .WithName("GetWeatherForecast")
// .WithOpenApi();

var products = new[]{
    new ProductTemp(1, "Smartphone X", "Latest flagship smartphone", 1, "TechCorp", 1, "Electronics", true, 999.99),
    new ProductTemp(2, "Smart TV", "4K UHD Smart TV", 1, "TechCorp", 1, "Electronics", true, 799.99),
    new ProductTemp(3, "Smart Fridge", "Energy-efficient smart fridge", 2, "EcoBrands", 2, "Home Appliances", true, 1499.99),
    new ProductTemp(4, "Smart Speaker", "Smart speaker with voice assistant", 2, "EcoBrands", 1, "Electronics", true, 199.99)   
};

var responseMessage = app.Configuration["Message"] ?? "";

app.MapGet("/products" , [Authorize] async(ProductsDbContext dbx)  => 
{

    var products = await dbx.Products
                            .Include(p => p.Brand)
                            .Include(p => p.Category)
                            .Select(p => new ProductDto
                            {
                                Id = p.Id,
                                Name = p.Name,
                                Description = p.Description,
                                Brand = new BrandDto
                                {
                                    Id = p.Brand.Id,
                                    Name = p.Brand.Name
                                },
                                Category = new CategoryDto
                                {
                                    Id = p.Category.Id,
                                    Name = p.Category.Name,
                                    Description = p.Category.Description
                                },
                                InStock = p.InStock,
                                Price = (decimal)p.Price
                            })
                            .ToListAsync();
    return Results.Ok(products);
    // return await dbx.Products
    //                 .Include(p => p.Brand)
    //                 .Include(p => p.Category)
    //                 .ToListAsync();

                    
});
// .WithName("GetProducts")
// .WithOpenApi();
app.MapGet("/products/{id}", [Authorize] async(ProductsDbContext dbx, int id) =>
{
    // var p = products.FirstOrDefault(p => p.Id == id);
    // if (product == null)
    // {
    //     return Results.NotFound();
    // }
    // return Results.Ok(product);

    var product = await dbx.Products
                           .Include(p => p.Brand)
                           .Include(p => p.Category)
                           .FirstOrDefaultAsync(p => p.Id == id);
    if (product == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(product);
    
});

app.MapPost("/products", [Authorize] async (ProductsDbContext dbx, ProductDto dto) =>
{
    var product = new Product
    {
        Name = dto.Name,
        Description = dto.Description,
        Brand = new Brand
        {
            Id = dto.Brand.Id,
            Name = dto.Brand.Name
        },
        Category = new Category
        {
            Id = dto.Category.Id,
            Name = dto.Category.Name,
            Description = dto.Category.Description
        },
        InStock = dto.InStock,
        Price = (double)dto.Price
    };
    await dbx.Products.AddAsync(product);
    await dbx.SaveChangesAsync();
    return responseMessage;
});

app.Run();

// record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
// {
//     public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
// }

record ProductTemp(int Id, string? Name, string? Description, int BrandId, string? BrandName, int CategoryId, string? CategoryName, bool InStock, double Price);
//public record ProductDto(string Name, string Description, int BrandId, int CategoryId, bool InStock, double Price);