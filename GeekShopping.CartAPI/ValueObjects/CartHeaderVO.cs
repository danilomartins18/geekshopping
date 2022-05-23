namespace GeekShopping.CartAPI.ValueObjects
{
    public class CartHeaderVO 
    {
        public long Id { get; set; } = default;
        public string UserId { get; set; } = string.Empty;
        public string? CouponCode { get; set; }
    }
}
