using Microsoft.AspNetCore.Userization;
using Microsoft.AspNetCore.Mvc;
using BountyHuntersBlog.Services.Interfaces;

namespace BountyHuntersBlog.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Userize(Roles = "Admin")]
    public class TagsController : Controller
    {
        private readonly ITagService _tags;
        public TagsController(ITagService tags) => _tags = tags;

        public async Task<IActionResult> Index()
            => View(await _tags.AllAsync());

        public IActionResult Create() => View();

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return View();
            await _tags.CreateAsync(name.Trim());
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var t = await _tags.GetAsync(id);
            if (t == null) return NotFound();
            return View(t);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return View(await _tags.GetAsync(id) ?? null);
            await _tags.UpdateAsync(id, name.Trim());
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _tags.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}