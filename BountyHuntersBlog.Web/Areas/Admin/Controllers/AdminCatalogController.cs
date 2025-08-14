using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BountyHuntersBlog.Services.Interfaces;
using BountyHuntersBlog.Services.DTOs;

namespace BountyHuntersBlog.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator")]
    public class AdminCatalogController : Controller
    {
        private readonly ICategoryService _categories;
        private readonly ITagService _tags;

        public AdminCatalogController(ICategoryService categories, ITagService tags)
        {
            _categories = categories;
            _tags = tags;
        }

        // ===== Categories =====
        [HttpGet]
        public async Task<IActionResult> Categories()
        {
            var list = await _categories.AllAsync();
            return View(list);
        }

        [HttpGet]
        public async Task<IActionResult> CategoryEdit(int? id)
        {
            if (id == null) return View(new CategoryDto());
            var dto = await _categories.GetAsync(id.Value);
            if (dto == null) return NotFound();
            return View(dto);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> CategoryEdit(CategoryDto dto)
        {
            if (!ModelState.IsValid) return View(dto);

            if (dto.Id == 0) dto.Id = await _categories.CreateAsync(dto);
            else await _categories.UpdateAsync(dto);

            return RedirectToAction(nameof(Categories));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> CategoryDelete(int id)
        {
            await _categories.SoftDeleteAsync(id);
            return RedirectToAction(nameof(Categories));
        }

        // ===== Tags =====
        [HttpGet]
        public async Task<IActionResult> Tags()
        {
            var list = await _tags.AllAsync();
            return View(list);
        }

        [HttpGet]
        public async Task<IActionResult> TagEdit(int? id)
        {
            if (id == null) return View(new TagDto());
            var dto = await _tags.GetAsync(id.Value);
            if (dto == null) return NotFound();
            return View(dto);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> TagEdit(TagDto dto)
        {
            if (!ModelState.IsValid) return View(dto);

            if (dto.Id == 0) dto.Id = await _tags.CreateAsync(dto);
            else await _tags.UpdateAsync(dto);

            return RedirectToAction(nameof(Tags));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> TagDelete(int id)
        {
            await _tags.SoftDeleteAsync(id);
            return RedirectToAction(nameof(Tags));
        }
    }
}
