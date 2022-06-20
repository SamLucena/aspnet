using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WellBooks.Data;
using WellBooks.Models;
using WellBooks.ViewModels;

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
            loadCategories();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(IFormCollection collection)
        {
            loadCategories();
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

        public IActionResult Edit(int? id)
        {
            if (id == null) { 
                return RedirectToAction("Index", "Product"); 
            }
            loadCategories();
            var product = _db.Products.Include(x => x.Category).FirstOrDefault(x => x.Id == id);
            ViewBag.ProductId = id;
            return View(new ProductCreateViewModel()
            {
                Category = product.Category,
                Description = product.Description,
                ImgUrl = product.ImgUrl,
                Name = product.Name,
                Price = product.Price
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(IFormCollection collection, int id)
        {
            var product = new Product()
            {
                Id = id,
                Name = collection["Name"],
                Category = _db.Categories.Find(int.Parse(collection["Category"].Single())),
                Description = collection["Description"],
                ImgUrl = collection["ImgUrl"],
                Price = double.Parse(collection["Price"])
            };
            _db.Products.Update(product);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index", "Product");
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Product");
            }
            var product = _db.Products.Include(x => x.Category).FirstOrDefault(x => x.Id == id);
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Product obj)
        {
            _db.Products.Remove(obj);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index", "Product");
        }

        public void loadCategories()
        {
            ViewBag.categories = _db.Categories.ToList<Category>();
        }
    }
}
