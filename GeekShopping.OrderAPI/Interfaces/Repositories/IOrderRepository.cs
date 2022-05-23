using GeekShopping.OrderAPI.Models;

namespace GeekShopping.OrderAPI.Interfaces.Repositories
{
    public interface IOrderRepository
    {
        Task<bool> AddOrder(OrderHeader header);
        Task UpdateOrderPaymentStatus(long orderHeaderId, bool paid);
    }
}
