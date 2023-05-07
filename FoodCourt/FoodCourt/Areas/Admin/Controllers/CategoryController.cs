using FoodCourt.Core.ExtensionMethods;
using FoodCourt.Data;
using FoodCourt.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FoodCourt.Areas.Admin.Controllers
{
    public class CategoryController : AdminControllerBase
    {
        private readonly ApplicationDbContext _db;
        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var categories = await _db.Categories.ToListAsync();
            return View(categories);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var categoryExists = await _db.Categories.AnyAsync(m => m.Name == model.Name);
            if (categoryExists)
            {
                ModelState.AddModelError("Name", "Category already exists.");
                return View(model);
            }

            _db.Categories.Add(new()
            {
                Name = model.Name,
                Description = model.Description
            });
            await _db.SaveChangesAsync();
            TempData.SetMessage(MessageType.Success, "Category inserted successfully.");
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit([FromRoute] int id)
        {
            var category = await _db.Categories.FindAsync(id);
            if(category is null)
            {
                return NotFound();
            }

            var model = new CategoryCreateViewModel
            {
                Name = category.Name,
                Description = category.Description
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromForm] CategoryCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var category = await _db.Categories.FindAsync(id);
            if (category is null)
            {
                return NotFound();
            }

            // Check if the updateed name is already taken.
            var categoryExists = await _db.Categories.AnyAsync(m => m.Name == model.Name && m.Id != id);
            if (categoryExists)
            {
                ModelState.AddModelError("Name", "Category already exists.");
                return View(model);
            }

            category.Name = model.Name;
            category.Description = model.Description;
            await _db.SaveChangesAsync();
            TempData.SetMessage(MessageType.Success, "Category updated successfully.");
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var category = await _db.Categories.FirstOrDefaultAsync(m => m.Id == id && m.IsDefault == false);
            if(category is null)
            {
                return NotFound();
            }

            _db.Categories.Remove(category);
            await _db.SaveChangesAsync();
            TempData.SetMessage(MessageType.Info, "Category deleted successfully.");
            return RedirectToAction(nameof(Index));
        }
    }
}
