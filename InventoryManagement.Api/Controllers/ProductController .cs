using InventoryManagement.Application.Dtos.Products;
using InventoryManagement.Application.Services;
using InventoryManagement.Domain.Entities;
using InventoryManagement.Infraestructure;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _service;

        public ProductController(ProductService service)
        {
            _service = service;
        }

        [HttpGet(Name = "GetProducts")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> Get()
        {
            var productsFromDb = await _service.GetProducts();
           
            return Ok(productsFromDb);
        }

        [HttpGet("{id}", Name = "GetProduct")]
        public async Task<ActionResult<ProductDto>> Get(int id)
        {
            var productFromDb = await _service.GetProduct(id);
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
                var productDb = await _service.CreateProduct(model);
                return CreatedAtRoute("GetProduct", new { id = productDb.Id }, productDb);
            }
            return BadRequest(ModelState);
        }

        [HttpPut("{id}", Name = "UpdateProduct")]
        public async Task<IActionResult> Update(int id, [FromBody] EditProduct model)
        {
            if (model == null || id != model.Id)
            {
                return BadRequest("Product data is invalid");
            }

            var productFromDb = await _service.GetProduct(id);
            if (productFromDb == null)
            {
                return NotFound("Product not found");
            }

            if (ModelState.IsValid)
            {
                await _service.EditProduct(model);
                return Ok(await _service.GetProduct(id));
            }

            return BadRequest(ModelState);
        }

        [HttpDelete("{id}", Name = "DeleteProduct")]
        public async Task<IActionResult> Delete(int id)
        {
            var productFromDb = await _service.GetProduct(id);
            if (productFromDb == null)
            {
                return NotFound("Product not found");
            }

            await _service.DeleteProduct(id);
            return Ok("Product deleted");
        }
    }
}
