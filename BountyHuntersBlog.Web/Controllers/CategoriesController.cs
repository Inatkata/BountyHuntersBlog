using AutoMapper;
using BountyHuntersBlog.Services.Interfaces;
using BountyHuntersBlog.ViewModels.Category;
using Microsoft.AspNetCore.Mvc;

namespace BountyHuntersBlog.Web
{
public class CategoriesController : Controller
{
    private readonly ICategoryService _categories;
    private readonly IMissionService _missions; // за да покажем мисиите в категорията
    private readonly IMapper _mapper;

    public CategoriesController(ICategoryService categories, IMissionService missions, IMapper mapper)
    {
        _categories = categories;
        _missions = missions;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var list = (await _categories.AllAsync()).ToList();

        var vmList = list.Select(_mapper.Map<CategoryViewModel>).ToList();
        return View(vmList);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var dto = await _categories.GetAsync(id);
        if (dto == null) return NotFound();

        var vm = _mapper.Map<CategoryViewModel>(dto);

        // вземаме и мисиите в тази категория (ако искаме да ги покажем)
        var (items, _) = await _missions.SearchPagedAsync(null, id, null, 1, 100);
        vm.Missions = items.ToList();

        return View(vm);
    }
}
}
