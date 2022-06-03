using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WellBooks.Data;
using WellBooks.Models;

namespace WellBooks.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ProductController(ApplicationDbContext db)
        {
            _db = db;   
        }

        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Details(int? id)
        {
            if (id == null) { return RedirectToAction("Index", "Home"); }
            var product = _db.Products.Include(x => x.Category).FirstOrDefault(x => x.Id == id);
            
            return View(product);
        }
        //IFormCollection : Itens do formulário
    }
}
