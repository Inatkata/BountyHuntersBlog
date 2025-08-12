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

   
}
