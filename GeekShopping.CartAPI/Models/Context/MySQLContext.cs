using Microsoft.EntityFrameworkCore;

namespace GeekShopping.CartAPI.Models.Context
{
    public class MySQLContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<CartDetail> CartDetails { get; set; }
        public DbSet<CartHeader> CartHeaders { get; set; }

        public MySQLContext() { }

        public MySQLContext(DbContextOptions<MySQLContext> options) : base(options) { }
    }
}
