using GeekShopping.CartAPI.ValueObjects;

namespace GeekShopping.CartAPI.Interfaces.Repositories
{
    public interface ICartRepository
    {
        Task<CartVO> FindCartByUserId(string userId);
        Task<CartVO> InsertOrUpdate(CartVO vo);
        Task<bool> Remove(long cartDetailId);
        Task<bool> ApplyCoupon(string userId, string couponCode);
        Task<bool> RemoveCoupon(string userId);
        Task<bool> Clear(string userId);
    }
}
