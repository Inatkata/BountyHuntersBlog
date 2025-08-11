using AutoMapper;
using BountyHuntersBlog.Services.Interfaces;
using BountyHuntersBlog.Services.DTOs;
using BountyHuntersBlog.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

public class CategoriesController : Controller
{
    private readonly ICategoryService _categories;
    private readonly IMapper _mapper;

    public CategoriesController(ICategoryService categories, IMapper mapper)
    {
        _categories = categories;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
        => View((await _categories.AllAsync()).Select(_mapper.Map<CategoryViewModel>).ToList());

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var dto = await _categories.GetAsync(id);
        if (dto == null) return NotFound();
        return View(_mapper.Map<CategoryViewModel>(dto));
    }

    [Authorize]
    [HttpGet] public IActionResult Create() => View(new CategoryViewModel());

    [Authorize]
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CategoryViewModel vm)
    {
        if (!ModelState.IsValid) return View(vm);
        var id = await _categories.CreateAsync(new CategoryDto { Name = vm.Name });
        return RedirectToAction(nameof(Details), new { id });
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var dto = await _categories.GetAsync(id);
        if (dto == null) return NotFound();
        return View(_mapper.Map<CategoryViewModel>(dto));
    }

    [Authorize]
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(CategoryViewModel vm)
    {
        if (!ModelState.IsValid) return View(vm);
        var ok = await _categories.UpdateAsync(new CategoryDto { Id = vm.Id, Name = vm.Name });
        if (!ok) return NotFound();
        return RedirectToAction(nameof(Details), new { id = vm.Id });
    }

    [Authorize]
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        await _categories.SoftDeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
