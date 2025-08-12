using AutoMapper;
using BountyHuntersBlog.Services.DTOs;
using BountyHuntersBlog.Services.Interfaces;
using BountyHuntersBlog.ViewModels.Admin.Tags;
using BountyHuntersBlog.ViewModels.Missions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace BountyHuntersBlog.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminTagsController : Controller
    {
        private readonly ITagService _tags;
        private readonly IMapper _mapper;
        private readonly IMissionService _missions;

        public AdminTagsController(ITagService tags, IMapper mapper, IMissionService missions)
        {
            _tags = tags;
            _mapper = mapper;
            _missions = missions;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var list = (await _tags.AllAsync())
                .Select(_mapper.Map<AdminTagListItemVM>)
                .ToList();
            return View(list);
        }

        [HttpGet]
        public IActionResult Create() => View(new AdminTagFormVM());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AdminTagFormVM vm)
        {
            if (!ModelState.IsValid) return View(vm);

            await _tags.CreateAsync(new TagDto
            {
                Name = vm.Name,
                IsDeleted = vm.IsDeleted
            });

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int missionId)
        {
            var mission = await _missions.GetByIdAsync(missionId);
            if (mission == null) return NotFound();

            var allTags = (await _tags.AllAsync()).ToList();
            var selected = mission.TagIds.ToHashSet();

            var vm = new MissionEditViewModel
            {
                Id = mission.Id,
                Title = mission.Title,
                Tags = allTags.Select(t => new SelectListItem(
                    text: t.Name,
                    value: t.Id.ToString(),
                    selected: selected.Contains(t.Id)
                )).ToList()
            };

            return View(vm);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AdminTagFormVM vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var ok = await _tags.UpdateAsync(new TagDto
            {
                Id = vm.Id,
                Name = vm.Name,
                IsDeleted = vm.IsDeleted
            });

            if (!ok) return NotFound();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _tags.SoftDeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
