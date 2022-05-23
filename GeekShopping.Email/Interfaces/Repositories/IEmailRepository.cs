using GeekShopping.Email.Messages;

namespace GeekShopping.OrderAPI.Interfaces.Repositories
{
    public interface IEmailRepository
    {
        Task LogEmail(UpdatePaymentResultMessage message);
    }
}
