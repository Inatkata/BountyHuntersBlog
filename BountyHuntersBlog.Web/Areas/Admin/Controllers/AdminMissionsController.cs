using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using BountyHuntersBlog.Services.Interfaces;
using BountyHuntersBlog.Services.DTOs;
using BountyHuntersBlog.ViewModels.Admin.Missions;

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

        public AdminMissionsController(
            IMissionService missions,
            ICategoryService categories,
            ITagService tags,
            IMapper mapper)
        {
            _missions = missions;
            _categories = categories;
            _tags = tags;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            var (items, _) = await _missions.SearchPagedAsync(null, null, null, page: 1, pageSize: 50);
            return View(items);
        }
        // GET: /Admin/AdminMissions/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var dto = await _missions.GetByIdAsync(id);
            if (dto == null) return NotFound();

            var vm = new AdminMissionFormVM
            {
                Id = dto.Id,
                Title = dto.Title,
                Description = dto.Description,
                ImageUrl = dto.ImageUrl,
                CategoryId = dto.CategoryId,
                TagIds = dto.TagIds?.ToList() ?? new List<int>(),
                IsCompleted = dto.IsCompleted,
                IsDeleted = dto.IsDeleted
            };

            await PopulateListsAsync(vm);
            return View(vm);
        }

        // POST: /Admin/AdminMissions/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AdminMissionFormVM vm)
        {
            if (!ModelState.IsValid)
            {
                await PopulateListsAsync(vm);
                return View(vm);
            }

            var update = new MissionDto
            {
                Id = vm.Id,
                Title = vm.Title,
                Description = vm.Description,
                ImageUrl = vm.ImageUrl,
                CategoryId = vm.CategoryId,
                TagIds = vm.TagIds?.ToList() ?? new List<int>(),
                IsCompleted = vm.IsCompleted,
                IsDeleted = vm.IsDeleted
            };

            var ok = await _missions.UpdateAsync(update);
            if (!ok) return NotFound();

            return RedirectToAction(nameof(Index));
        }

        // helper: fill dropdowns/multiselect without ViewBag

        // optional, if not yet present
        [HttpGet]

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var vm = new AdminMissionFormVM();
            await PopulateListsAsync(vm);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AdminMissionFormVM vm)
        {
            if (!ModelState.IsValid)
            {
                await PopulateListsAsync(vm);
                return View(vm);
            }

            var dto = new MissionDto
            {
                Title = vm.Title,
                Description = vm.Description,
                ImageUrl = vm.ImageUrl,
                CategoryId = vm.CategoryId,
                TagIds = vm.TagIds?.ToList() ?? new List<int>(),
                IsCompleted = vm.IsCompleted,
                IsDeleted = vm.IsDeleted
            };

            await _missions.CreateAsync(dto);
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var dto = await _missions.GetByIdAsync(id);
            if (dto == null) return NotFound();
            return View(dto); 
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ok = await _missions.SoftDeleteAsync(id); // SOFT DELETE here
            if (!ok) return NotFound();
            return RedirectToAction(nameof(Index));
        }
        private async Task PopulateListsAsync(AdminMissionFormVM vm)
        {
            var categories = (await _categories.AllAsync()).ToList();
            var tags = (await _tags.AllAsync()).ToList();
            var selected = vm.TagIds?.ToHashSet() ?? new HashSet<int>();

            vm.Categories = categories
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name,
                    Selected = c.Id == vm.CategoryId
                })
                .ToList();

            vm.Tags = tags
                .Select(t => new SelectListItem
                {
                    Value = t.Id.ToString(),
                    Text = t.Name,
                    Selected = selected.Contains(t.Id)
                })
                .ToList();
        }

    }
}
