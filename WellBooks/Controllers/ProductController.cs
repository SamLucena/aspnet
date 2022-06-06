using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WellBooks.Data;
using WellBooks.Models;

namespace WellBooks.Controllers
{
    [Authorize(Policy = "EmployeeOnly")]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ProductController(ApplicationDbContext db)
        {
            _db = db;   
        }

        public IActionResult Index()
        {
            return View(_db.Products);
        }

        [AllowAnonymous]
        public IActionResult Details(int? id)
        {
            if (id == null) { return RedirectToAction("Index", "Home"); }
            var product = _db.Products.Include(x => x.Category).FirstOrDefault(x => x.Id == id);
            
            return View(product);
        }

        public IActionResult Create()
        {
            loadCatories();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(IFormCollection collection)
        {
            loadCatories();
            Product product = new Product()
            {
                Name = collection["Name"].Single(),
                Price = double.Parse(collection["Price"].Single()),
                Description = collection["Description"].Single(),   
                ImgUrl = collection["ImgUrl"].Single(),
                Category = _db.Categories.Find(int.Parse(collection["Category"].Single()))
            };
            _db.Products.Add(product);
            _db.SaveChanges();
            return RedirectToAction("Index", "Product");
        }

        public void loadCatories()
        {
            ViewBag.categories = _db.Categories.ToList<Category>();
        }

        //IFormCollection : Itens do formulário
    }
}
