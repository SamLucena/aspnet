using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WellBooks.Controllers
{
    [Authorize(Policy = "EmployeeOnly")]
    public class ProductController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

    }
}
