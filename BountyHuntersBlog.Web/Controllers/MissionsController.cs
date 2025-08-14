using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using AutoMapper;
using BountyHuntersBlog.Services.Interfaces;
using BountyHuntersBlog.Services.DTOs;
using BountyHuntersBlog.ViewModels.Missions;

namespace BountyHuntersBlog.Web.Controllers
{
    public class MissionsController : BaseController
    {
        private readonly IMissionService _missions;
        private readonly ICategoryService _categories;
        private readonly ITagService _tags;
        private readonly IMapper _mapper;

        public MissionsController(
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

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Index(string? q, int? categoryId, int? tagId, int page = 1, int pageSize = 10)
        {
            var (items, total) = await _missions.SearchPagedAsync(q, categoryId, tagId, page, pageSize);

            var vm = new MissionIndexViewModel
            {
                Q = q,
                CategoryId = categoryId,
                TagId = tagId,
                // FIX: map DTO -> VM
                Items = _mapper.Map<List<MissionListItemVm>>(items),
                Page = page,
                PageSize = pageSize,
                TotalCount = total
            };
            return View(vm);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var dto = await _missions.GetByIdAsync(id);
            if (dto == null) return NotFound();

            
            var vm = _mapper.Map<MissionDetailsViewModel>(dto);
            return View(vm);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var vm = new MissionEditViewModel();
            await PopulateListsAsync(vm);
            return View(vm);
        }

        [Authorize]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MissionEditViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                await PopulateListsAsync(vm);
                return View(vm);
            }

            // FIX: без ToEditDto() – ползваме AutoMapper
            var editDto = _mapper.Map<MissionEditDto>(vm);
            await _missions.CreateAsync(editDto);

            TempData["Success"] = "Mission created.";
            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var dto = await _missions.GetByIdAsync(id);
            if (dto == null) return NotFound();

            // FIX: без FromDto() – ползваме AutoMapper
            var vm = _mapper.Map<MissionEditViewModel>(dto);
            await PopulateListsAsync(vm);
            return View(vm);
        }

        [Authorize]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MissionEditViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                await PopulateListsAsync(vm);
                return View(vm);
            }

            // FIX: без ToDto() – ползваме AutoMapper
            var updateDto = _mapper.Map<MissionDto>(vm);
            await _missions.UpdateAsync(updateDto);

            TempData["Success"] = "Mission updated.";
            return RedirectToAction(nameof(Details), new { id = vm.Id });
        }

        [Authorize]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _missions.SoftDeleteAsync(id);
            TempData["Success"] = "Mission deleted.";
            return RedirectToAction(nameof(Index));
        }

        // helpers
        private async Task PopulateListsAsync(MissionEditViewModel vm)
        {
            var categories = (await _categories.AllAsync()).ToList();
            var tags = (await _tags.AllAsync()).ToList();
            var selected = vm.TagIds?.ToHashSet() ?? new HashSet<int>();

            vm.Categories = categories
                .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name, Selected = c.Id == vm.CategoryId })
                .ToList();

            vm.Tags = tags
                .Select(t => new SelectListItem { Value = t.Id.ToString(), Text = t.Name, Selected = selected.Contains(t.Id) })
                .ToList();
        }
    }
}
