using Microsoft.EntityFrameworkCore;

namespace GeekShopping.OrderAPI.Models.Context
{
    public class MySQLContext : DbContext
    {
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<OrderHeader> OrderHeaders { get; set; }

        public MySQLContext() { }

        public MySQLContext(DbContextOptions<MySQLContext> options) : base(options) { }
    }
}
