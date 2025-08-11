using AutoMapper;
using BountyHuntersBlog.Services.DTOs;
using BountyHuntersBlog.Services.Interfaces;
using BountyHuntersBlog.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BountyHuntersBlog.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminCategoriesController : Controller
    {
        private readonly ICategoryService _categories;
        private readonly IMapper _mapper;
        public AdminCategoriesController(ICategoryService categories, IMapper mapper)
        {
            _categories = categories; _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
            => View((await _categories.AllAsync()).Select(_mapper.Map<CategoryViewModel>).ToList());

        [HttpGet]
        public IActionResult Create() => View(new CategoryViewModel());

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);
            await _categories.CreateAsync(new CategoryDto { Name = vm.Name });
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var dto = await _categories.GetAsync(id);
            if (dto == null) return NotFound();
            return View(_mapper.Map<CategoryViewModel>(dto));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CategoryViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);
            var ok = await _categories.UpdateAsync(new CategoryDto { Id = vm.Id, Name = vm.Name });
            if (!ok) return NotFound();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _categories.SoftDeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
