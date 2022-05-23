using GeekShopping.Email.Messages;
using GeekShopping.Email.Models.Context;
using GeekShopping.OrderAPI.Interfaces.Repositories;
using GeekShopping.OrderAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace GeekShopping.Email.Repositories
{
    public class EmailRepository : IEmailRepository
    {
        private readonly DbContextOptions<MySQLContext> _context;

        public EmailRepository(DbContextOptions<MySQLContext> context)
        {
            _context = context;
        }

        public async Task LogEmail(UpdatePaymentResultMessage message)
        {
            EmailLog email = new()
            {
                Email = message.Email,
                SentDate = DateTime.Now,
                Log = $"Order - {message.OrderId} has been created successfully!"
            };

            await using var db = new MySQLContext(_context);
            db.Emails.Add(email);
            await db.SaveChangesAsync();
        }
    }
}

