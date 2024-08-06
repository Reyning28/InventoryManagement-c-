using InventoryManagement.Application.Dtos.Products;
using InventoryManagement.Application.Services;
using InventoryManagement.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagement.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductService _service;
        private readonly CategoryService _categoryService;

        public ProductController(ProductService service, CategoryService categoryService)
        {
            _service = service;
            _categoryService = categoryService;
        }

        // GET: Product
        public async Task<IActionResult> Index()
        {
            var products = await _service.GetProducts();
            return View(products);
        }

        // GET: Product/Create
        public async Task<IActionResult> Create()
        {
            var categories = await _categoryService.GetCategories();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            ViewData["Title"] = "Create Product";
            return View();
        }

        // POST: Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Price,Stock,CategoryId")] CreateProduct model)
        {
            if (ModelState.IsValid)
            {
                await _service.CreateProduct(model);
                return RedirectToAction(nameof(Index));
            }

            var categories = await _categoryService.GetCategories();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View(model);
        }

        // GET: Product/Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _service.GetProduct(id.Value);
            if (product == null)
            {
                return NotFound();
            }

            var vm = new EditProduct
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock,
                CategoryId = product.CategoryId
            };

            var categories = await _categoryService.GetCategories();
            ViewBag.Categories = new SelectList(categories, "Id", "Name", product.CategoryId);
            ViewData["Title"] = "Edit Product";
            return View(vm);
        }

        // POST: Product/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,Stock,CategoryId")] EditProduct model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _service.EditProduct(model);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await _service.GetProduct(id) == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            var categories = await _categoryService.GetCategories();
            ViewBag.Categories = new SelectList(categories, "Id", "Name", model.CategoryId);
            return View(model);
        }
    }
}
