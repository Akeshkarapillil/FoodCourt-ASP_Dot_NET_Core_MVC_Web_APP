using FoodCourt.Core.ExtensionMethods;
using FoodCourt.Data;
using FoodCourt.Models.Entities;
using FoodCourt.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections;

namespace FoodCourt.Areas.Admin.Controllers
{
    public class ProductController : AdminControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ApplicationDbContext _db;

        public ProductController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _db.Products.Include(c => c.Category).OrderBy(p => p.Id).ToListAsync();
            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> Detail([FromRoute] int id)
        {
            var product = await _db.Products.FindAsync(id);
            if (product is null)
            {
                return NotFound();
            }
            var productImages = await _db.ProductImages.Where(m => m.Product == product).ToListAsync();

            ViewBag.ProductImages = productImages;
            return View(product);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var categories = await _db.Categories.ToListAsync();
            var model = new ProductViewModel
            {
                Categories = new SelectList(categories, "Id", "Name")
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ProductViewModel model)
        {
            var categories = await _db.Categories.ToListAsync();
            if (!ModelState.IsValid)
            {
                model.Categories = new SelectList(categories, "Id", "Name");
                return View(model);
            }
            var slugExists = await _db.Products.AnyAsync(m => m.Slug == model.Slug);
            if (slugExists)
            {
                ModelState.AddModelError("Slug", "Slug already exists.");
                return View(model);
            }

            var product = new Product
            {
                Name = model.Name,
                Slug = model.Slug,
                ShortDescription = model.ShortDescription,
                Description = model.Description,
                Price = model.Price,
                DateCreated = DateTime.UtcNow,
                CategoryId = model.CategoryId,
                Status = model.Status
            };

            var productImages = new List<ProductImage>();
            foreach (var file in model.ProductImages)
            {
                string path = Path.Combine(_webHostEnvironment.WebRootPath, "media");
                string fileName = await file.SaveFile(path);
                productImages.Add(new() { 
                    ImageUrl = fileName,
                    Product = product
                });
            }

            _db.Products.Add(product);
            _db.ProductImages.AddRange(productImages);

            await _db.SaveChangesAsync();
            TempData.SetMessage(MessageType.Success, "Product inserted successfully.");
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit([FromRoute] int id)
        {
            var categories = await _db.Categories.ToListAsync();
            var product = await _db.Products.FindAsync(id);
            if(product is null)
            {
                return NotFound();
            }


            var model = new ProductViewModel
            {
                Name = product.Name,
                Slug = product.Slug,
                ShortDescription = product.ShortDescription,
                Description = product.Description,
                Price = product.Price,
                Status = product.Status,
                Categories = new SelectList(categories, "Id", "Name", "3")
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromForm] ProductViewModel model)
        {
            var product = await _db.Products.FindAsync(id);
            if (product is null)
            {
                return NotFound();
            }

            var categories = await _db.Categories.ToListAsync();
          
            if (!ModelState.IsValid)
            {
                model.Categories = new SelectList(categories, "Id", "Name", product.CategoryId);
                return View(model);
            }

            var slugExists = await _db.Products.AnyAsync(m => m.Slug == model.Slug && m.Id != id);
            if (slugExists)
            {
                ModelState.AddModelError("Slug", "Slug already exists.");
                return View(model);
            }

            if (model.ProductImages != null)
            {
                var productImages = new List<ProductImage>();
                foreach (var file in model.ProductImages)
                {
                    string path = Path.Combine(_webHostEnvironment.WebRootPath, "media");
                    string fileName = await file.SaveFile(path);
                    productImages.Add(new()
                    {
                        ImageUrl = fileName,
                        Product = product
                    });
                }
                _db.ProductImages.AddRange(productImages);
            }

            product.Name = model.Name;
            product.Slug = model.Slug;
            product.ShortDescription = model.ShortDescription;
            product.Description = model.Description;
            product.Price = model.Price;
            product.CategoryId = model.CategoryId == product.CategoryId ? product.CategoryId : model.CategoryId;
            product.Status = model.Status;
            product.DateUpdated = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            TempData.SetMessage(MessageType.Success, "Product updated successfully.");
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var product = await _db.Products.FirstOrDefaultAsync(p => p.Id == id);
            
            if (product is null)
            {
                return NotFound();
            }

            var productImages = await _db.ProductImages.Where(m => m.Product == product).ToListAsync();

            if(productImages.Count > 0)
            {
                foreach (var image in productImages)
                {
                    if (!string.IsNullOrEmpty(image.ImageUrl))
                    {
                        var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "media", image.ImageUrl);
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }
                    }
                }
                _db.ProductImages.RemoveRange(productImages);
            }
            _db.Products.Remove(product);
            await _db.SaveChangesAsync();
            TempData.SetMessage(MessageType.Info, "Product deleted successfully.");
            return RedirectToAction(nameof(Index));
        }
    }

    public class ProductWithImages<TProduct, List> 
    {
        public TProduct Product { get; set;}

        public List<ProductImage> ProductImages { get; set;}
    }
}
