using Microsoft.AspNetCore.Mvc;
using Pronia.DAL;
using Pronia.ViewModels;

namespace Pronia.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            HomeVM homeVM = new HomeVM
            {
                MainSliders = _context.MainSliders,
                ShippingAreas = _context.ShippingAreas.ToList(),
                TestimonialArea = _context.TestimonialAreas.FirstOrDefault(),
                Testimonials = _context.Testimonials,
                Brands = _context.Brands,
                Banners = _context.Banners.OrderBy(o => o.Order)
            };


            return View(homeVM);


        }


    }
}