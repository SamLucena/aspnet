using WellBooks.Models;

namespace WellBooks.ViewModels
{
    public class CartViewModel
    {
        public User User { get; set; }
        public Order Order { get; set; }
        public List<Product> Products { get; set; }
    }
}
