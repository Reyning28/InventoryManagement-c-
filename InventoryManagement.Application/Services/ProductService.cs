using InventoryManagement.Application.Dtos.Products;
using InventoryManagement.Domain.Entities;
using InventoryManagement.Infraestructure;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryManagement.Application.Services
{
    public class ProductService
    {
        private readonly ApplicationDbContext _context;

        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }

        private async Task<Product> GetDbProduct(int id)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<ProductDto> GetProduct(int id)
        {
            var itemFromDb = await GetDbProduct(id);
            if (itemFromDb == null)
            {
                return null;
            }

            return new ProductDto
            {
                Name = itemFromDb.Name,
                Price = itemFromDb.Price,
                Stock = itemFromDb.Stock,
                CategoryId = itemFromDb.CategoryId
            };
        }

        public async Task<Product> CreateProduct(CreateProduct model)
        {
            var newItemDb = new Product
            {
                Name = model.Name,
                Price = model.Price,
                Stock = model.Stock,
                CategoryId = model.CategoryId
            };

            _context.Products.Add(newItemDb);
            await _context.SaveChangesAsync();
            return newItemDb;
        }

        public async Task<Product> EditProduct(EditProduct model)
        {
            var itemFromDb = await GetDbProduct(model.Id);
            if (itemFromDb == null)
            {
                return null;
            }

            itemFromDb.Name = model.Name;
            itemFromDb.Price = model.Price;
            itemFromDb.Stock = model.Stock;
            itemFromDb.CategoryId = model.CategoryId;

            _context.Products.Update(itemFromDb);
            await _context.SaveChangesAsync();
            return itemFromDb;
        }
        public async Task<bool> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return false;
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

