using GeekShopping.CouponAPI.ValueObjects;

namespace GeekShopping.CouponAPI.Interfaces.Repositories
{
    public interface ICouponRepository
    {
        Task<CouponVO> GetCouponByCouponCode(string couponCode);
    }
}
