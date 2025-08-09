using BountyHuntersBlog.Services.DTOs;
using BountyHuntersBlog.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BountyHuntersBlog.Web.Areas.Admin.Controllers
{
    public class AdminCategoriesController : Controller
    {
        private readonly ICategoryService _service;
        public AdminCategoriesController(ICategoryService service)
        {
            _service = service;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CategoryDto dto)
        {
            if (id != dto.Id) ModelState.AddModelError("", "Mismatched id.");
            if (!ModelState.IsValid) return View(dto);

            await _service.UpdateAsync(id, dto);
            return RedirectToAction(nameof(Index));
        }

    }
}
