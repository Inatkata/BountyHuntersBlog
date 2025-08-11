using AutoMapper;
using BountyHuntersBlog.Services.Interfaces;
using BountyHuntersBlog.Services.DTOs;
using BountyHuntersBlog.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

public class TagsController : Controller
{
    private readonly ITagService _tags;
    private readonly IMapper _mapper;


    public TagsController(ITagService tags, IMapper mapper)
    {
        _tags = tags;
        _mapper = mapper;
    }

    [HttpGet]
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var list = await _tags.AllAsync();
        var model = list.Select(t => _mapper.Map<TagViewModel>(t)).ToList();
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var dto = await _tags.GetAsync(id);
        if (dto == null) return NotFound();
        var vm = _mapper.Map<TagViewModel>(dto);
        return View(vm);
    }

    [Authorize]
    [HttpGet]
    public IActionResult Create() => View(new TagViewModel());



    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var dto = await _tags.GetAsync(id);
        if (dto == null) return NotFound();
        return View(_mapper.Map<TagViewModel>(dto));

    }

    [Authorize]
    [HttpGet] public IActionResult Create() => View(new TagViewModel());

    [Authorize]
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TagViewModel vm)
    {
        if (!ModelState.IsValid) return View(vm);
        var id = await _tags.CreateAsync(new TagDto { Name = vm.Name });
        return RedirectToAction(nameof(Details), new { id });
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var dto = await _tags.GetAsync(id);
        if (dto == null) return NotFound();
        return View(_mapper.Map<TagViewModel>(dto));
    }

    [Authorize]
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(TagViewModel vm)
    {
        if (!ModelState.IsValid) return View(vm);
        var ok = await _tags.UpdateAsync(new TagDto { Id = vm.Id, Name = vm.Name });
        if (!ok) return NotFound();
        return RedirectToAction(nameof(Details), new { id = vm.Id });
    }

    [Authorize]
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        await _tags.SoftDeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
