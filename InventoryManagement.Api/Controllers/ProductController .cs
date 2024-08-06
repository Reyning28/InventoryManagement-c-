using InventoryManagement.Api.Dtos.Products;
using InventoryManagement.Domain.Entities;
using InventoryManagement.Infraestructure;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet(Name = "GetProducts")]
        public ActionResult<IEnumerable<Product>> Get()
        {
            var productsFromDb = _context.Products.ToList();
            return Ok(productsFromDb);
        }

        [HttpGet("{id}", Name = "GetProduct")]
        public ActionResult<Product> Get(int id)
        {
            var productFromDb = _context.Products.FirstOrDefault(p => p.Id == id);
            if (productFromDb == null)
            {
                return NotFound("Product not found");
            }
            return Ok(productFromDb);
        }

        [HttpPost(Name = "CreateProduct")]
        public async Task<IActionResult> Create([FromBody] CreateProduct model)
        {
            if (model == null)
            {
                return BadRequest("Product data is null");
            }

            if (ModelState.IsValid)
            {
                var productDb = new Product
                {
                    Name = model.Name,
                    Price = model.Price,
                    Stock = model.Stock,
                    CategoryId = model.CategoryId
                };

                _context.Products.Add(productDb);
                await _context.SaveChangesAsync();
                return CreatedAtRoute("GetProduct", new { id = productDb.Id }, productDb);
            }
            return BadRequest(ModelState);
        }

        [HttpPut("{id}", Name = "UpdateProduct")]
        public async Task<IActionResult> Update(int id, [FromBody] Product model)
        {
            if (model == null || id != model.Id)
            {
                return BadRequest("Product data is invalid");
            }

            var productFromDb = await _context.Products.FindAsync(id);
            if (productFromDb == null)
            {
                return NotFound("Product not found");
            }

            if (ModelState.IsValid)
            {
                productFromDb.Name = model.Name;
                productFromDb.Price = model.Price;
                productFromDb.Stock = model.Stock;
                productFromDb.CategoryId = model.CategoryId;

                _context.Products.Update(productFromDb);
                await _context.SaveChangesAsync();
                return Ok(productFromDb);
            }

            return BadRequest(ModelState);
        }

        [HttpDelete("{id}", Name = "DeleteProduct")]
        public async Task<IActionResult> Delete(int id)
        {
            var productFromDb = await _context.Products.FindAsync(id);
            if (productFromDb == null)
            {
                return NotFound("Product not found");
            }

            _context.Products.Remove(productFromDb);
            await _context.SaveChangesAsync();
            return Ok("Product deleted");
        }
    }
}
