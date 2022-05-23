using GeekShopping.OrderAPI.Models.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeekShopping.OrderAPI.Models
{
    [Table("order_detail")]
    public class OrderDetail : BaseEntity
    {
        [Column("order_header_id")]
        public long OrderHeaderId { get; set; }
        [Column("product_id")]
        public long ProductId { get; set; }
        [Column("count")]
        public int Count { get; set; }
        [Column("product_name")]
        public string ProductName { get; set; }
        [Column("price")]
        public decimal Price { get; set; }


        [ForeignKey("OrderHeaderId")]
        public virtual OrderHeader OrderHeader { get; set; }
    }
}