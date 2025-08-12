using AutoMapper;
using BountyHuntersBlog.Services.DTOs;
using BountyHuntersBlog.Services.Interfaces;
using BountyHuntersBlog.ViewModels.Admin.Missions; // <-- Admin VMs
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
        { _missions = missions; _categories = categories; _tags = tags; _mapper = mapper; }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var (items, _) = await _missions.SearchPagedAsync(null, null, null, 1, 100); // проста листа
            var list = items.Select(d => _mapper.Map<AdminMissionListItemVM>(d)).ToList();
            return View(list); // @model IEnumerable<AdminMissionListItemVM>
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var vm = new AdminMissionFormVM
            {
                Categories = (await _categories.AllAsync()).Select(c => new SelectListItem(c.Name, c.Id.ToString())),
                Tags = (await _tags.AllAsync()).Select(t => new SelectListItem(t.Name, t.Id.ToString()))
            };
            return View(vm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AdminMissionFormVM vm)
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
                ImageUrl = vm.ImageUrl,
                CategoryId = vm.CategoryId,
                TagIds = vm.TagIds,
                IsCompleted = vm.IsCompleted,
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

            var vm = _mapper.Map<AdminMissionFormVM>(dto);
            vm.Categories = (await _categories.AllAsync()).Select(c => new SelectListItem(c.Name, c.Id.ToString()));
            vm.Tags = (await _tags.AllAsync()).Select(t => new SelectListItem(t.Name, t.Id.ToString()));
            return View(vm); // @model AdminMissionFormVM
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AdminMissionFormVM vm)
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
                ImageUrl = vm.ImageUrl,
                CategoryId = vm.CategoryId,
                TagIds = vm.TagIds,
                IsCompleted = vm.IsCompleted
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
