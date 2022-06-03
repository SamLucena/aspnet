using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WellBooks.Data;
using WellBooks.Models;
using WellBooks.Utils;
using WellBooks.ViewModel;
using WellBooks.ViewModels;

namespace WellBooks.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _db;

        public AccountController(ApplicationDbContext db)
        {
            _db = db;
        }

        [Authorize]
        public IActionResult Profile()
        {
            return View(_db.Users.Where(x => x.Email == User.Identity.Name).First());
        }

        [Authorize]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserEditViewModel obj)
        {
            
            var user = _db.Users.Where(x => x.Email == User.Identity.Name).First();
            user.Name = obj.Name;
            user.Password = obj.Password; 
            _db.Users.Update(user);
            _db.SaveChanges();
            TempData["info"] = "Informações alteradas com êxito";
            return RedirectToAction("Profile", user);
            
        }

        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                TempData["error"] = "Já identificado";
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginAsync(LoginViewModel obj)
        {
            User user = new User();
            var possibleUsers = _db.Users.Where(x => x.Email == obj.Email && x.Password == obj.Password);
            if (!possibleUsers.Any())
            {
                ModelState.AddModelError("Password", "Email e/ou senha inválidos");
            }
            else
            {
                user = possibleUsers.First();
            }

            if (user != null && ModelState.IsValid)
            {

                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim("Role", user.Role)
                };
                if (user.Role == "employee") claims.Add(new Claim("Role", "customer"));

                var identity = new ClaimsIdentity(claims, Constants.AUTH_COOKIE_NAME);

                ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);

                var authProperties = new AuthenticationProperties() { IsPersistent = obj.RememberMe };

                await HttpContext.SignInAsync(Constants.AUTH_COOKIE_NAME, claimsPrincipal, authProperties);
                TempData["success"] = "Autenticado com sucesso :)";
                return RedirectToAction("Index", "Home");
            }
            return View(obj);
        }

        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                TempData["error"] = "Já identificado";
                return RedirectToAction("Index", "Home");
            }
            return View();  
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterAsync(SignUpViewModel obj)
        {
            User user = new User() { 
                Email = obj.Email,
                Password = obj.Password,
                Name = obj.Name,
                Role = obj.Role
            };
            await _db.Users.AddAsync(user);
            _db.SaveChanges();
            TempData["success"] = "Criado com sucesso!";
            return RedirectToAction("Login", "Account");
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(Constants.AUTH_COOKIE_NAME);
            return RedirectToAction("Index", "Home");
        }

    }
}
