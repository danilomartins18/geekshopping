using GeekShopping.CartAPI.ValueObjects;

namespace GeekShopping.CartAPI.Interfaces.Repositories
{
    public interface ICouponRepository
    {
        Task<CouponVO> GetCoupon(string couponCode, string token);
    }
}
