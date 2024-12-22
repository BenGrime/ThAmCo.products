using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using ThAmCo.Products.Api.Data; // Update with your actual namespace

namespace ThAmCo.Products.Api.Controllers // Update with your actual namespace
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ProductsDbContext _context;

        public ProductsController(ProductsDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

         [HttpPost]
        public async Task<IActionResult> PostProduct(ProductDto dto)
        {
            var product = new Product
            {
                Name = dto.Name,
                Id = dto.Id,
                Description = dto.Description,
                BrandName = dto.BrandName,
                BrandDescription = dto.BrandDescription,
                CategoryName = dto.CategoryName,
                CategoryDescription = dto.CategoryDescription,
                InStock = dto.InStock,
                Price = (double)dto.Price
            };
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}