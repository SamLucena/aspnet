using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WellBooks.Models
{
    public class OrderDetail
    {
        public Product Product { get; set; }
        public Order Order { get; set; }
        public int Amount { get; set; }

        public double GetTotal()
        {
            return Product.Price * Amount;
        }
    }
}
