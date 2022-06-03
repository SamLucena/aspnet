using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WellBooks.Data;
using WellBooks.Models;

namespace WellBooks.Controllers
{
    [Authorize(Policy = "EmployeeOnly")]
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CategoryController(ApplicationDbContext db)
        {
            _db = db;   
        }

        public IActionResult Index()
        {
            return View(_db.Categories);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Create")]
        public async Task<IActionResult> Create(Category obj)
        {
            await _db.Categories.AddAsync(obj);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index", "Category");
        }

        public IActionResult Edit(int? id)
        {
            if(id == null)
            {
                TempData["error"] = "Não encontramos essa categoria";
                return RedirectToAction("Index", "Category");
            }
            var category = _db.Categories.FirstOrDefault(c => c.Id == id);
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Edit")]
        public async Task<IActionResult> Edit(Category obj)
        {
            _db.Categories.Update(obj);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index", "Category");
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                TempData["error"] = "Não encontramos essa categoria";
                return RedirectToAction("Index");
            }
            var category = _db.Categories.FirstOrDefault(c => c.Id == id);
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> Delete(Category obj)
        {
            _db.Categories.Remove(obj);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
