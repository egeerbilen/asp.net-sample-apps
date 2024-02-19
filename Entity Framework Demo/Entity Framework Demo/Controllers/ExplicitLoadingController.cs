using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Entity_Framework_Demo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExplicitLoadingController : ControllerBase
    {
        private readonly YourDbContext _context;

        public ProductsController(YourDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            // Tüm ürünleri almak yerine sadece ihtiyaç duyulan özellikleri yükler
            var products = await _context.Products
                .Select(p => new Product
                {
                    ProductId = p.ProductId,
                    Name = p.Name
                    // Diğer özellikleri buraya ekleyebilirsiniz
                })
                .ToListAsync();

            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            // Belirli bir ürünü yükler
            var product = await _context.Products
                .Where(p => p.ProductId == id)
                .Select(p => new Product
                {
                    ProductId = p.ProductId,
                    Name = p.Name
                    // Diğer özellikleri buraya ekleyebilirsiniz
                })
                .FirstOrDefaultAsync();

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }
    }
}
