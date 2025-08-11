using Microsoft.AspNetCore.Userization;
using Microsoft.AspNetCore.Mvc;
using BountyHuntersBlog.Services.Interfaces;
using BountyHuntersBlog.Services.DTOs;

namespace BountyHuntersBlog.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Userize(Roles = "Admin")]
    public class AdminTagsController : Controller
    {
        private readonly ITagService _service;
        public AdminTagsController(ITagService service) => _service = service;

        public async Task<IActionResult> Index(int page = 1, int pageSize = 20)
        {
            var items = await _service.GetAllAsync(page, pageSize); // FIX: add paging args
            return View(items);
        }

        [HttpGet]
        public IActionResult Create() => View(new TagDto());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TagDto dto)
        {
            if (!ModelState.IsValid) return View(dto);
            await _service.CreateAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var dto = await _service.GetByIdAsync(id);
            if (dto == null) return NotFound();
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TagDto dto)
        {
            if (!ModelState.IsValid) return View(dto);
            await _service.UpdateAsync(dto.Id, dto); // FIX: id + dto
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}