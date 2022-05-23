using GeekShopping.Web.ViewModels;

namespace GeekShopping.Web.Interfaces.Services
{
    public interface ICouponService
    {
        Task<CouponViewModel> GetCoupon(string code, string token);
     }
}
