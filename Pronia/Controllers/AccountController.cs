using Microsoft.AspNetCore.Mvc;

namespace Pronia.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
