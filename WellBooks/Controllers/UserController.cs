using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WellBooks.Data;
using WellBooks.Models;
using WellBooks.ViewModel;

namespace WellBooks.Controllers
{
    [Authorize(Policy = "EmployeeOnly")]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _db;

        public UserController(ApplicationDbContext db)
        {
            _db = db;   
        }

        public IActionResult Index(string? role)
        {
            if(role == null)
                return View(_db.Users);
            else
            {
                return View(_db.Users.Where(x => x.Role == role));
            }
        }

        public IActionResult Edit(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var user = _db.Users.FirstOrDefault(x => x.Id == id);
            var userViewModel = new SignUpViewModel() {
                Name = user.Name,
                Email = user.Email,
                Password = user.Password,
                Role = user.Role,
            };
            ViewBag.UserId = id;
            return View(userViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(IFormCollection collection, int id)
        {
            var user = new User()
            {
                Id = id,
                Name = collection["Name"],
                Email = collection["Email"],
                Password = collection["Password"],
                Role = collection["Role"]
            };
            _db.Users.Update(user);
            await _db.SaveChangesAsync();
            TempData["success"] = "Usuário atualizado com sucesso!";
            return RedirectToAction("Index", "User");   
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = _db.Users.FirstOrDefault(x => x.Id == id);
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(User obj)
        {
            _db.Users.Remove(obj);
            await _db.SaveChangesAsync();
            TempData["success"] = "Usuário deletado com sucesso!";
            return RedirectToAction("Index", "User");
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormCollection collection)
        {
            var user = new User()
            {
                Name = collection["Name"],
                Email = collection["Email"],
                Password = collection["Password"],
                Role = collection["Role"]
            };
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            TempData["success"] = "Usuário criado com sucesso!";
            return RedirectToAction("Index", "User");
        }
    }
}
