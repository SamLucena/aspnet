using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WellBooks.Data;
using WellBooks.Models;
using WellBooks.Models.Enums;
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
            if(orderby == null) return View(_db.Orders.Include(x => x.User));
            if (orderby == "desc")
            {
                var orders = _db.Orders.OrderByDescending(x => x.Moment).Include(x => x.User);
                return View(orders);
            }else if(orderby == "success")
            {
                var orders = _db.Orders.Where(x => x.Status == OrderStatus.SUCCESS).Include(x => x.User);
                return View(orders);
            }else if (orderby == "waiting")
            {
                var orders = _db.Orders.Where(x => x.Status == OrderStatus.WAITING).Include(x => x.User);
                return View(orders);
            }else if (orderby == "pending")
            {
                var orders = _db.Orders.Where(x => x.Status == OrderStatus.PENDING).Include(x => x.User);
                return View(orders);
            }else if (orderby == "delivered")
            {
                var orders = _db.Orders.Where(x => x.Status == OrderStatus.DELIVERED).Include(x => x.User);
                return View(orders);
            }
            return View(_db.Orders.Include(x => x.User));
        }

        public IActionResult Edit(int? id)
        {
            if (id == null) return NotFound();
            var order = _db.Orders.Include(x => x.User).FirstOrDefault(x => x.Id == id);
            return View(order);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();
            var order = _db.Orders.Include(x => x.User).FirstOrDefault(x => x.Id == id);
            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Order obj)
        {
            _db.Remove(obj);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index", "Orders");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Order obj)
        {
            var entity = _db.Orders.Include(x => x.User).FirstOrDefault(x => x.Id == obj.Id);
            entity.Status = obj.Status;
            _db.Orders.Update(entity);
            await _db.SaveChangesAsync();
            TempData["info"] = "Status atualizado com sucesso!";
            return RedirectToAction("Index", "Orders");
        }
    }
}
