using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using BountyHuntersBlog.Services.Interfaces;
using BountyHuntersBlog.ViewModels.Category;

namespace BountyHuntersBlog.Web.Controllers
{
    public class CategoriesController : BaseController
    {
        private readonly ICategoryService _categories;
        private readonly IMissionService _missions;
        private readonly IMapper _mapper;

        public CategoriesController(ICategoryService categories, IMissionService missions, IMapper mapper)
        { _categories = categories; _missions = missions; _mapper = mapper; }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var list = (await _categories.AllAsync()).ToList();
            var vmList = list.Select(_mapper.Map<CategoryViewModel>).ToList();
            return View(vmList);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var dto = await _categories.GetAsync(id);
            if (dto == null) return NotFound();

            var vm = _mapper.Map<CategoryViewModel>(dto);
            var (items, _) = await _missions.SearchPagedAsync(null, id, null, 1, 100);
            vm.Missions = items.ToList();

            return View(vm);
        }
    }
}