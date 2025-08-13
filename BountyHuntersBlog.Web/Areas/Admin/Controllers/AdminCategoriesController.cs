// Areas/Admin/Controllers/AdminCategoriesController.cs
using AutoMapper;
using BountyHuntersBlog.Services.DTOs;
using BountyHuntersBlog.Services.Interfaces;
using BountyHuntersBlog.ViewModels.Admin.Categories;
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
        { _categories = categories; _mapper = mapper; }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var items = (await _categories.AllAsync())
                .Select(_mapper.Map<AdminCategoryListItemVM>)
                .OrderBy(x => x.Name)
                .ToList();
            return View(items);
        }

        [HttpGet]
        public IActionResult Create() => View(new AdminCategoryFormVM());

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AdminCategoryFormVM vm)
        {
            if (!ModelState.IsValid) return View(vm);
            await _categories.CreateAsync(new CategoryDto { Name = vm.Name, IsDeleted = vm.IsDeleted });
            TempData["Success"] = "Category created.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var dto = await _categories.GetAsync(id);
            if (dto == null) return NotFound();
            return View(_mapper.Map<AdminCategoryFormVM>(dto));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AdminCategoryFormVM vm)
        {
            if (!ModelState.IsValid) return View(vm);
            var ok = await _categories.UpdateAsync(new CategoryDto { Id = vm.Id, Name = vm.Name, IsDeleted = vm.IsDeleted });
            if (!ok) return NotFound();
            TempData["Success"] = "Category updated.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _categories.SoftDeleteAsync(id);
            TempData[ok ? "Success" : "Error"] = ok ? "Category deleted." : "Category not found.";
            return RedirectToAction(nameof(Index));
        }
    }
}
