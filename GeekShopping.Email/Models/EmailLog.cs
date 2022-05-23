using GeekShopping.Email.Models.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeekShopping.OrderAPI.Models
{
    [Table("email_log")]
    public class EmailLog : BaseEntity
    {
        [Column("email")]
        public string Email { get; set; }
        [Column("log")]
        public string Log { get; set; }
        [Column("sent_date")]
        public DateTime SentDate { get; set; }
    }
}