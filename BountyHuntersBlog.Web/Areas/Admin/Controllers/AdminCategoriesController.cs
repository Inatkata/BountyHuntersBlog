using AutoMapper;
using BountyHuntersBlog.Services.DTOs;
using BountyHuntersBlog.Services.Interfaces;
using BountyHuntersBlog.ViewModels.Admin.Categories;
using BountyHuntersBlog.ViewModels.Category;
using Microsoft.AspNetCore.Mvc;

namespace BountyHuntersBlog.Web.Areas.Admin.Controllers
{
    public class AdminCategoriesController : BaseAdminController
    {
        private readonly ICategoryService _categories;
        private readonly IMapper _mapper;

        public AdminCategoriesController(ICategoryService categories, IMapper mapper)
        {
            _categories = categories;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var list = (await _categories.AllAsync())
                .Select(_mapper.Map<AdminCategoryListItemVM>)
                .ToList();
            return View(list);
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

            var vm = _mapper.Map<AdminCategoryFormVM>(dto);
            return View(vm);
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
            await _categories.SoftDeleteAsync(id);
            TempData["Success"] = "Category deleted.";
            return RedirectToAction(nameof(Index));
        }
    }
}
