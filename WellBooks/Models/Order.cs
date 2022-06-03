using System.ComponentModel.DataAnnotations;
using WellBooks.Models.Enums;

namespace WellBooks.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public DateTime Moment { get; set; }
        public OrderStatus Status { get; set; }
    }
}
