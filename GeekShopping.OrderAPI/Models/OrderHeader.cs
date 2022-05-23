﻿using GeekShopping.OrderAPI.Models.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeekShopping.OrderAPI.Models
{
    [Table("order_header")]
    public class OrderHeader : BaseEntity
    {
        [Column("user_id")]
        public string UserId { get; set; }
        [Column("coupon_code")]
        public string CouponCode { get; set; }
        [Column("purchase_amount")]
        public decimal PurchaseAmount { get; set; }
        [Column("discount_amount")]
        public decimal DiscountAmount { get; set; }
        [Column("first_name")]
        public string FirstName { get; set; }
        [Column("last_name")]
        public string LastName { get; set; }
        [Column("order_date")]
        public DateTime OrderDate { get; set; }
        [Column("order_time")]
        public DateTime OrderTime{ get; set; }
        [Column("phone")]
        public string Phone { get; set; }
        [Column("email")]
        public string Email { get; set; }
        [Column("card_number")]
        public string CardNumber { get; set; }
        [Column("cvv")]
        public string CVV { get; set; }
        [Column("expiry_month_year")]
        public string ExpiryMonthYear { get; set; }
        [Column("total_itens")]
        public int TotalItens { get; set; }
        [Column("payment_status")]
        public bool PaymentStatus { get; set; }
        public List<OrderDetail>? OrderDetails { get; set; }
    }
}
