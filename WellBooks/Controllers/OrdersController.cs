using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WellBooks.Data;
using WellBooks.Models;
using WellBooks.Utils;
using WellBooks.ViewModels;

namespace WellBooks.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly OrderDetailRepository orderDetailRepository = new(); 

        public OrdersController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Payment()
        {
            List<Item> items = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            ViewBag.total = items.Sum(x => x.Quantity * x.Product.Price);
            ViewBag.products = items.Select(x => x.Product).ToList();
            ViewBag.count = items.Count;
            return View(items);
        }

        public IActionResult ConfirmOrder()
        {
            var order = new Order();
            order.User = _db.Users.FirstOrDefault(x => x.Email == User.Identity.Name);
            order.Status = Models.Enums.OrderStatus.PENDING;
            order.Moment = DateTime.Now;
            List<Item> items = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            foreach(Item item in items)
            {
                order.Details.Add(new OrderDetail()
                {
                    Amount = item.Quantity,
                    Product = item.Product,
                    Order = order
                });
            }
            _db.Orders.Add(order);  
            _db.SaveChanges();
            foreach(var orderDetail in order.Details)
            {
                orderDetailRepository.Add(orderDetail);
            };
            _db.SaveChanges();
            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home");
        }

        [Authorize(Policy = "EmployeeOnly")]
        public IActionResult Index(string? orderby)
        {
            if(orderby == "desc")
            {
                var orders = _db.Orders.OrderByDescending(x => x.Moment).Include(x => x.User);
                return View(orders);
            }
            return View(_db.Orders.Include(x => x.User));
        }
    }
}
