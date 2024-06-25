using Microsoft.EntityFrameworkCore;
using ShoppingCartService.Models;

namespace ShoppingCartService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Item> Items { get; set; }
        public DbSet<Cart> Carts { get; set; }
    }
}