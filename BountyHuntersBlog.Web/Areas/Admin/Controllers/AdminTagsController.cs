using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BountyHuntersBlog.Services.Interfaces;
using BountyHuntersBlog.Services.DTOs;

namespace BountyHuntersBlog.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class TagsController : Controller
    {
        private readonly ITagService _service;
        public TagsController(ITagService service) => _service = service;

        public async Task<IActionResult> Index()
            => View(await _service.GetAllAsync(1, int.MaxValue));

        public IActionResult Create() => View(new TagDto());
        [HttpPost]
        public async Task<IActionResult> Create(TagDto dto)
        { if (!ModelState.IsValid) return View(dto); await _service.CreateAsync(dto); return RedirectToAction(nameof(Index)); }

        public async Task<IActionResult> Edit(int id)
        { var t = await _service.GetByIdAsync(id); if (t == null) return NotFound(); return View(t); }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TagDto dto)
        {
            if (id != dto.Id) ModelState.AddModelError("", "Mismatched id.");
            if (!ModelState.IsValid) return View(dto);

            await _service.UpdateAsync(id, dto);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        { await _service.DeleteAsync(id); return RedirectToAction(nameof(Index)); }
    }
}