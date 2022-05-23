namespace GeekShopping.OrderAPI.Messages
{
    public class ProductMessage
    {
        public long Id { get; set; } = default;
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Description { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
    }
}
