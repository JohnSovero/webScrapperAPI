using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Infraestructure
{    
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Supplier> Suppliers { get; set; }
    }
}