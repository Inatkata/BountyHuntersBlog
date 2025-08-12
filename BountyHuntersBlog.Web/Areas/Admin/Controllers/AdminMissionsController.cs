
using AutoMapper;
using BountyHuntersBlog.Services.DTOs;
using BountyHuntersBlog.Services.Interfaces;
using BountyHuntersBlog.ViewModels.Missions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace BountyHuntersBlog.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminMissionsController : Controller
    {
        private readonly IMissionService _missions;
        private readonly ICategoryService _categories;
        private readonly ITagService _tags;
        private readonly IMapper _mapper;

        public AdminMissionsController(IMissionService missions, ICategoryService categories, ITagService tags, IMapper mapper)
        {
            _missions = missions; _categories = categories; _tags = tags; _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, int pageSize = 20)
        {
            var (items, total) = await _missions.SearchPagedAsync(null, null, null, page, pageSize);
            var vm = new MissionIndexViewModel
            {
                Page = page,
                PageSize = pageSize,
                TotalCount = total,
                Items = items.Select(d => new MissionListItemViewModel
                {
                    Id = d.Id,
                    Title = d.Title,
                    Description = d.Description,
                    CreatedOn = DateTime.UtcNow
                }).ToList()
            };
            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var vm = new MissionEditViewModel
            {
                Categories = (await _categories.AllAsync()).Select(c => new SelectListItem(c.Name, c.Id.ToString())),
                Tags = (await _tags.AllAsync()).Select(t => new SelectListItem(t.Name, t.Id.ToString()))
            };
            return View(vm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MissionEditViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Categories = (await _categories.AllAsync()).Select(c => new SelectListItem(c.Name, c.Id.ToString()));
                vm.Tags = (await _tags.AllAsync()).Select(t => new SelectListItem(t.Name, t.Id.ToString()));
                return View(vm);
            }

            var dto = new MissionDto
            {
                Title = vm.Title,
                Description = vm.Description,
                CategoryId = vm.CategoryId,
                TagIds = vm.TagIds,
                UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)

            };

            var id = await _missions.CreateAsync(dto);
            return RedirectToAction(nameof(Edit), new { id });
        }

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
            return View(vm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MissionEditViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Categories = (await _categories.AllAsync()).Select(c => new SelectListItem(c.Name, c.Id.ToString()));
                vm.Tags = (await _tags.AllAsync()).Select(t => new SelectListItem(t.Name, t.Id.ToString()));
                return View(vm);
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
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _missions.SoftDeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
