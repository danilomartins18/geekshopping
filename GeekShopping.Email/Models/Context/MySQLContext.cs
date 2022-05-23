using GeekShopping.OrderAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace GeekShopping.Email.Models.Context
{
    public class MySQLContext : DbContext
    {
        public DbSet<EmailLog> Emails { get; set; }

        public MySQLContext(DbContextOptions<MySQLContext> options) : base(options) { }
    }
}
