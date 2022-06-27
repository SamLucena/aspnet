using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;
using WellBooks.Data;
using WellBooks.Models;
using WellBooks.Utils;
using WellBooks.ViewModel;

namespace WellBooks.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        [AllowAnonymous]
        public IActionResult Index(int? categoryId)
        {
            ViewBag.Categories = _db.Categories;
            if(categoryId == null)
                ViewBag.Products = _db.Products;
            else {
                ViewBag.Products = _db.Products.Where(x => x.Category == _db.Categories.Find(categoryId));
                var products = (IEnumerable<Product>)ViewBag.Products;
                if (products.Any() == false)
                {
                    TempData["info"] = "Nenhum produto com essa categoria :(";
                    ViewBag.Products = _db.Products;
                }
            }
               
                
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(IFormCollection collection)
        {
            var query = collection["query"];
            ViewBag.Categories = _db.Categories;
            ViewBag.Products = _db.Products.Where(x => x.Name.Contains(query));
            var products = (IEnumerable<Product>)ViewBag.Products;
            if (!products.Any())
            {
                TempData["info"] = "Nenhum produto encontrado!";
                ViewBag.Products = _db.Products;
            }
            return View();
        }

        [AllowAnonymous]
        public IActionResult About()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Contact(Contact obj)
        {
            await _db.Contacts.AddAsync(obj);
            await _db.SaveChangesAsync();
            TempData["success"] = "Mensagem enviada com sucesso!";
            return RedirectToAction("Index", "Home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}