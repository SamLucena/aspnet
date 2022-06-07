using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WellBooks.Data;
using WellBooks.Models;
using WellBooks.ViewModels;

namespace WellBooks.Controllers
{
    [Authorize(Policy = "CustomerOnly")]
    public class OrderDetailsController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly CartViewModel _cart = new();

        public OrderDetailsController(ApplicationDbContext db)
        {
            _db = db;   
        }

        public IActionResult Cart(int? id)
        {
            var currentUser = _db.Users.Where(x => x.Email == User.Identity.Name).First();

            CartViewModel model = new();
            Order order = new Order();
            var orderDetailList = _db.OrderDetails
                .Include(x => x.Product)
                .Include(x => x.Order);
            var list = orderDetailList.Where(x => x.Order.User == currentUser).ToList();
            var products = new List<Product>();
            foreach(var item in list)
            {
                products.Add(item.Product);
            }
            if (id != null) {
                products.Add(_db.Products.Find(id));
            }
            model.Products = products;
            model.Order = _db.Orders.FirstOrDefault(x => x.User == currentUser);
            model.User = currentUser;
            model.Order.Details = list;
            return View(model);
        }
        /*
         var list = _db.OrderDetails
                .Include(x => x.Product)
                .Include(x => x.Order);
            var products = new List<Product>();
            var order = new Order();
            foreach (var item in list)
            {
                products.Add(item.Product);
                order = item.Order;
            }
            model.Products = products;
            model.Order = order;
            
            model.User = currentUser;
         */
    }
}
