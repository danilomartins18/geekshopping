using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeekShopping.OrderAPI.Models.Base
{
    public class BaseEntity
    {
        [Key]
        [Column("Id")]
        public long Id { get; set; }
    }
}
