namespace GeekShopping.CartAPI.ValueObjects
{
    public class CartDetailVO
    {
        public long Id { get; set; }
        public long CartHeaderId { get; set; }
        public long ProductId { get; set; }
        public int Count { get; set; }
        public CartHeaderVO? CartHeader { get; set; }
        public ProductVO? Product { get; set; }
    }
}
