using AutoMapper;
using BountyHuntersBlog.Services.DTOs;
using BountyHuntersBlog.Services.Interfaces;
using BountyHuntersBlog.ViewModels.Comments;
using BountyHuntersBlog.ViewModels.Missions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BountyHuntersBlog.Web.Controllers
{
    public class MissionsController : Controller
    {
        private readonly IMissionService _missions;
        private readonly IMapper _mapper;

        public MissionsController(IMissionService missions, IMapper mapper)
        {
            _missions = missions;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? q, int? categoryId, int? tagId, int page = 1, int pageSize = 10)
        {
            var (items, total) = await _missions.SearchPagedAsync(q, categoryId, tagId, page, pageSize);

            var vm = new MissionIndexViewModel()
            {
                Q = q,
                CategoryId = categoryId,
                TagId = tagId,
                Page = page,
                PageSize = pageSize,
                TotalCount = total,
                Items = _mapper.Map<List<MissionListItemVm>>(items),
                Categories = await _missions.GetCategoriesSelectListAsync(),
                Tags = await _missions.GetTagsSelectListAsync()
            };

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var dto = await _missions.GetDetailsAsync(id);
            if (dto == null) return NotFound();

            var vm = _mapper.Map<MissionDetailsViewModel>(dto);
            return View(vm);
        }

        // Likes се обработват през LikesController (AJAX). Тук няма like actions.

        [Authorize, HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment(AddCommentRequest model)
        {
            if (!ModelState.IsValid)
                return RedirectToAction(nameof(Details), new { id = model.MissionId });

            await _missions.AddCommentAsync(model.MissionId, model.Content, User);
            return RedirectToAction(nameof(Details), new { id = model.MissionId });
        }

        [Authorize, HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteComment(int id, int missionId)
        {
            await _missions.DeleteCommentAsync(id, User);
            return RedirectToAction(nameof(Details), new { id = missionId });
        }

        [Authorize, HttpGet]
        public async Task<IActionResult> Create()
        {
            var vm = new MissionEditViewModel
            {
                Categories = await _missions.GetCategoriesSelectListAsync(),
                Tags = await _missions.GetTagsSelectListAsync()
            };
            return View(vm);
        }

        [Authorize, HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MissionEditViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Categories = await _missions.GetCategoriesSelectListAsync();
                vm.Tags = await _missions.GetTagsSelectListAsync();
                return View(vm);
            }

            var dto = _mapper.Map<MissionEditDto>(vm);
            await _missions.CreateAsync(dto);

            return RedirectToAction(nameof(Index));
        }

        [Authorize, HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var dto = await _missions.GetEditAsync(id);
            if (dto == null) return NotFound();

            var vm = _mapper.Map<MissionEditViewModel>(dto);
            vm.Categories = await _missions.GetCategoriesSelectListAsync();
            vm.Tags = await _missions.GetTagsSelectListAsync();
            return View(vm);
        }

        [Authorize, HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MissionEditViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Categories = await _missions.GetCategoriesSelectListAsync();
                vm.Tags = await _missions.GetTagsSelectListAsync();
                return View(vm);
            }

            var dto = _mapper.Map<MissionEditDto>(vm);
            await _missions.EditAsync(dto);

            return RedirectToAction(nameof(Details), new { id = vm.Id });
        }
    }
}
