using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Pronia.DAL;
using Pronia.Models;
using Pronia.Utilies.Extensions;
using Pronia.ViewModels;

namespace Pronia.Areas.Manage.Controllers
{
    [Area(nameof(Manage))]
    public class ProductsController : Controller
    {
        readonly AppDbContext _context;
        readonly IWebHostEnvironment _env;

        public ProductsController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Index()
        {
           
            return View(_context.Products.Include(p => p.ProductColors).ThenInclude(pc => pc.Color).Include(p => p.ProductSizes).ThenInclude(ps => ps.Size).Include(p => p.ProductCategories).ThenInclude(pc => pc.Category).Include(p => p.ProductImages));
        } 

        
        public IActionResult Delete(int? id)
        {
            if (id is null || id <= 0) return BadRequest();
            Product product = _context.Products.Find(id);
            if (product == null) return NotFound();
            product.IsDeleted = true;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        
        public IActionResult Create()
        {
            ViewBag.Colors = new SelectList(_context.Colors,nameof(Color.Id),nameof(Color.Name));
            ViewBag.Sizes = new SelectList(_context.Sizes,nameof(Size.Id),nameof(Size.Name));
            ViewBag.Categories = new SelectList(_context.Categories,nameof(Category.Id),nameof(Category.Name));
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateProductVM cp)
        {

            ViewBag.Colors = new SelectList(_context.Colors, nameof(Color.Id), nameof(Color.Name));
            ViewBag.Sizes = new SelectList(_context.Sizes, nameof(Size.Id), nameof(Size.Name));
            ViewBag.Categories = new SelectList(_context.Categories, nameof(Category.Id), nameof(Category.Name));

            if (!ModelState.IsValid)
            {
                
                return View();
            }

            var sizes = _context.Sizes.Where(s => cp.SizeIds.Contains(s.Id));
            var colors = _context.Colors.Where(col => cp.ColorIds.Contains(col.Id));
            var categories = _context.Categories.Where(c => cp.CategoryIds.Contains(c.Id));
            Product newProduct = new Product
            {
                Name = cp.Name,
                Desc = cp.Desc,
                SKU = Guid.NewGuid().ToString(),
                CostPrice = cp.CostPrice,
                SellPrice = cp.SellPrice,
                Discount = cp.Discount,
                IsDeleted = false,
                Date = DateTime.Now,

            };
            var coverImg = cp.CoverImage;
            var hoverImg = cp.HoverImage;
            var otherImgs = cp.OtherImages;
            if (coverImg?.CheckType("image/") == false)
            {
                ModelState.AddModelError("CoverImage", "Cover file isn't image file");
            }
            if (coverImg?.CheckSize(300) == false)
            {
                ModelState.AddModelError("CoverImage", "The size of the image can not be large from 300 KB");
            }


            if (hoverImg?.CheckType("image/") == false)
            {
                ModelState.AddModelError("HoverImage", "Hover file isn't image file");
            }
            if (hoverImg?.CheckSize(300) == false)
            {
                ModelState.AddModelError("HoverImage", "The size of the image can not be large from 300 KB");
            }

            List<ProductImage> images = new List<ProductImage>();

            images.Add(new ProductImage { Image = coverImg.SaveFile(Path.Combine(_env.WebRootPath, "assets", "images", "product")), IsCover = true, Product = newProduct });
            images.Add(new ProductImage { Image = hoverImg.SaveFile(Path.Combine(_env.WebRootPath, "assets", "images", "product")), IsCover = false, Product = newProduct });



            foreach (IFormFile item in otherImgs)
            {
                if (item?.CheckType("image/") == false)
                {
                    ModelState.AddModelError("CoverImage", "Cover file isn't image file");
                }
                if (item?.CheckSize(300) == false)
                {
                    ModelState.AddModelError("CoverImage", "The size of the image can not be large from 300 KB");
                }

                images.Add(new ProductImage { Image = item.SaveFile(Path.Combine(_env.WebRootPath, "assets", "images", "product")), IsCover = null, Product = newProduct });
            }

            

            newProduct.ProductImages = images;


            _context.Products.Add(newProduct);  


            foreach (var item in sizes)
            {
                _context.ProductSizes.Add(new ProductSize {
                    Product = newProduct,
                    SizeId = item.Id
                });
            }
            foreach (var item in colors)
            {
                _context.ProductColors.Add(new ProductColor {
                    Product = newProduct,
                    ColorId = item.Id
                });
            }
            foreach (var item in categories)
            {
                _context.ProductCategories.Add(new ProductCategory {
                    Product = newProduct,
                    CategoryId = item.Id
                });
            }


            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
