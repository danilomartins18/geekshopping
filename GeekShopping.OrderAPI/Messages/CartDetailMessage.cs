namespace GeekShopping.OrderAPI.Messages
{
    public class CartDetailMessage
    {
        public long Id { get; set; }
        public long CartHeaderId { get; set; }
        public long ProductId { get; set; }
        public int Count { get; set; }

        public virtual ProductMessage? Product { get; set; }
    }
}
