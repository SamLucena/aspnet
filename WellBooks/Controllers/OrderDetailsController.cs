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
        private CartViewModel _cart = new() { Products = new() };

        public OrderDetailsController(ApplicationDbContext db, ISession session)
        {
            _db = db;
        }

        public IActionResult Cart(int? id)
        {
            if (id == null)
                return View(_cart);
            var product = _db.Products.FirstOrDefault(x => x.Id == id);
            _cart.Products.Add(product);
            return View(_cart);
        }

        public IActionResult RemoveItem(IFormCollection collection)
        {
            TempData["success"] = "Removido com sucesso!";
            return RedirectToAction("Cart", "OrderDetails");
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

        /*
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
            return View(model);*/
    }
}
