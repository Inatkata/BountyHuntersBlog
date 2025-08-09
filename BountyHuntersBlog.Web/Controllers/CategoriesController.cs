using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using BountyHuntersBlog.Services.Interfaces;
using BountyHuntersBlog.Services.DTOs;
using BountyHuntersBlog.ViewModels;

namespace BountyHuntersBlog.Web.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoryService _service;
        private readonly IMapper _mapper;

        public CategoriesController(ICategoryService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            const int pageSize = 10;
            var dtos = await _service.GetAllAsync(page, pageSize);
            var vms = _mapper.Map<IEnumerable<CategoryViewModel>>(dtos);
            return View(vms);
        }

        public async Task<IActionResult> Details(int id)
        {
            var dto = await _service.GetByIdAsync(id);
            if (dto == null) return NotFound();
            return View(_mapper.Map<CategoryViewModel>(dto));
        }

        [HttpGet]
        public IActionResult Create() => View(new CategoryViewModel());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);
            var dto = _mapper.Map<CategoryDto>(vm);
            await _service.CreateAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var dto = await _service.GetByIdAsync(id);
            if (dto == null) return NotFound();
            return View(_mapper.Map<CategoryViewModel>(dto));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CategoryViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);
            var dto = _mapper.Map<CategoryDto>(vm);
            await _service.UpdateAsync(vm.Id, dto);
            return RedirectToAction(nameof(Details), new { id = vm.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var dto = await _service.GetByIdAsync(id);
            if (dto == null) return NotFound();
            return View(_mapper.Map<CategoryViewModel>(dto));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _service.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}