using Employeees.Domain.Entities;
using Employees.Web.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet(Name = "GetProducts")]
        public IEnumerable<Product> Get()
        {
            var productsFromDb = _context.Products.ToList();
            return productsFromDb;
        }

        [HttpGet("{id}", Name = "GetProducts")]
        public ActionResult<Product> Get(int id)
        {
            var productsFromDb = _context.Employees.FirstOrDefault(p => p.Id == id);
            if (productsFromDb == null)
            {
                return NotFound("Product not found");
            }
        }
    }
}