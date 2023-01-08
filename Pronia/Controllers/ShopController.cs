using Microsoft.AspNetCore.Mvc;
using Pronia.DAL;

namespace Pronia.Controllers
{
    public class ShopController : Controller
    {
        readonly AppDbContext _context;

        public ShopController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Colors = _context.Colors.ToList();
            return View();
        }
    }
}
