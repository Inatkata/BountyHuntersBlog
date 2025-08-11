using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BountyHuntersBlog.Services.Interfaces;

namespace BountyHuntersBlog.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CategoriesController : Controller
    {
        private readonly ICategoryService _categories;
        public CategoriesController(ICategoryService categories) => _categories = categories;

        public async Task<IActionResult> Index()
            => View(await _categories.AllAsync());

        public IActionResult Create() => View();

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return View();
            await _categories.CreateAsync(name.Trim());
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var c = await _categories.GetAsync(id);
            if (c == null) return NotFound();
            return View(c);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return View(await _categories.GetAsync(id) ?? null);
            await _categories.UpdateAsync(id, name.Trim());
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _categories.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}