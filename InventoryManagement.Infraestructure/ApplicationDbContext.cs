using Microsoft.EntityFrameworkCore;

using InventoryManagement.Domain.Entities;

namespace InventoryManagement.Infraestructure
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

        
    }
}
