using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using InventoryManagement.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using InventoryManagement.Domain.Entities;
using InventoryManagement.Infraestructure;
using InventoryManagement.Web.ViewModels.Products;

namespace InventoryManagement.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Product
        public async Task<IActionResult> Index()
        {
            return View(await _context.Products.ToListAsync());
        }

        // GET: Product/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "Id", "Name");
            return View();
        }

        // POST: Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Price,Stock,CategoryId")] CreateProduct model)
        {
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

                // Verificar el Id del producto recién creado
                Console.WriteLine($"Producto creado con Id: {productDb.Id}");

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "Id", "Name");
            return View(model);
        }

        // GET: Product/Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dbitem = await _context.Products.FindAsync(id);
            if (dbitem == null)
            {
                return NotFound();
            }

            var vm = new EditProduct
            {
                Id = dbitem.Id,
                Name = dbitem.Name,
                Price = dbitem.Price,
                Stock = dbitem.Stock,
                CategoryId = dbitem.CategoryId
            };

            ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "Id", "Name", dbitem.CategoryId);
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
                    var dbitem = await _context.Products.FindAsync(id);
                    if (dbitem == null)
                    {
                        return NotFound();
                    }

                    dbitem.Name = model.Name;
                    dbitem.Price = model.Price;
                    dbitem.Stock = model.Stock;
                    dbitem.CategoryId = model.CategoryId;

                    _context.Products.Update(dbitem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Products.Any(e => e.Id == model.Id))
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

            ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "Id", "Name", model.CategoryId);
            return View(model);
        }
    }
}
