using System.Security.Claims;
using AutoMapper;
using BountyHuntersBlog.Services.DTOs;
using BountyHuntersBlog.Services.Interfaces;
using BountyHuntersBlog.ViewModels.Missions;

using BountyHuntersBlog.ViewModels.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

public class MissionsController : Controller
{
    private readonly IMissionService _missions;
    private readonly ICategoryService _categories;
    private readonly ITagService _tags;
    private readonly IMapper _mapper;
    private readonly ICommentService _comments;

    public MissionsController(IMissionService missions, ICategoryService categories, ITagService tags, IMapper mapper, ICommentService comments)
    {
        _missions = missions;
        _categories = categories;
        _tags = tags;
        _mapper = mapper;
        _comments = comments;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string? q, int? categoryId, int? tagId, int page = 1, int pageSize = 10)
    {
        var (items, total) = await _missions.SearchPagedAsync(q, categoryId, tagId, page, pageSize);

        var vm = new MissionIndexViewModel
        {
            Q = q,
            CategoryId = categoryId,
            TagId = tagId,
            Page = page,
            PageSize = pageSize,
            TotalCount = total,
            Items = items.Select(d => _mapper.Map<MissionListItemViewModel>(d)).ToList(),
            Categories = (await _categories.AllAsync()).Select(c => new SelectListItem(c.Name, c.Id.ToString())),
            Tags = (await _tags.AllAsync()).Select(t => new SelectListItem(t.Name, t.Id.ToString()))
        };
        return View(vm);
    }

    [HttpGet]
    // в MissionsController
    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var dto = await _missions.GetDetailsAsync(id);
        if (dto == null) return NotFound();

        var vm = _mapper.Map<MissionDetailsViewModel>(dto);

        // Load comments for the mission
        var comments = await _comments.GetForMissionAsync(id);
        vm.Comments = comments.Select(c => new MissionDetailsViewModel.CommentItem
        {
            Id = c.Id,
            Content = c.Content,
            CreatedOn = c.CreatedOn,
            UserId = c.UserId,
            UserDisplayName = c.UserDisplayName
        }).ToList();

        return View(vm);
    }


    [Authorize]
    [HttpGet]
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var vm = new MissionEditViewModel
        {
            Categories = (await _categories.AllAsync()).Select(c => new SelectListItem(c.Name, c.Id.ToString())),
            Tags = (await _tags.AllAsync()).Select(t => new SelectListItem(t.Name, t.Id.ToString()))
        };
        return View("Create", vm);
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(BountyHuntersBlog.ViewModels.Missions.MissionEditViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            vm.Categories = (await _categories.AllAsync()).Select(c => new SelectListItem(c.Name, c.Id.ToString()));
            vm.Tags = (await _tags.AllAsync()).Select(t => new SelectListItem(t.Name, t.Id.ToString()));
            return View("Create", vm);
        }
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var dto = new MissionDto
        {
            Title = vm.Title,
            Description = vm.Description,
            CategoryId = vm.CategoryId,
            TagIds = vm.TagIds,
            UserId = userId
        };
        var id = await _missions.CreateAsync(dto);
        return RedirectToAction(nameof(Details), new { id });
    }
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var dto = await _missions.GetByIdAsync(id);
        if (dto == null) return NotFound();

        var vm = new MissionEditViewModel
        {
            Id = dto.Id,
            Title = dto.Title,
            Description = dto.Description,
            CategoryId = dto.CategoryId,
            TagIds = dto.TagIds.ToList(),
            Categories = (await _categories.AllAsync()).Select(c => new SelectListItem(c.Name, c.Id.ToString())),
            Tags = (await _tags.AllAsync()).Select(t => new SelectListItem(t.Name, t.Id.ToString()))
        };
        return View("Edit", vm);
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(BountyHuntersBlog.ViewModels.Missions.MissionEditViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            vm.Categories = (await _categories.AllAsync()).Select(c => new SelectListItem(c.Name, c.Id.ToString()));
            vm.Tags = (await _tags.AllAsync()).Select(t => new SelectListItem(t.Name, t.Id.ToString()));
            return View("Edit", vm);
        }

        var ok = await _missions.UpdateAsync(new MissionDto
        {
            Id = vm.Id,
            Title = vm.Title,
            Description = vm.Description,
            CategoryId = vm.CategoryId,
            TagIds = vm.TagIds
        });
        if (!ok) return NotFound();

        return RedirectToAction(nameof(Details), new { id = vm.Id });
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var ok = await _missions.SoftDeleteAsync(id);
        if (!ok) return NotFound();
        return RedirectToAction(nameof(Index));
    }
}
