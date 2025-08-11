using AutoMapper;
using BountyHuntersBlog.Services.DTOs;
using BountyHuntersBlog.Services.Interfaces;
using BountyHuntersBlog.ViewModels;
using BountyHuntersBlog.ViewModels.Missions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace BountyHuntersBlog.Web.Controllers
{
    public class MissionsController : Controller
    {
        private readonly IMissionService _service;
        private readonly IMapper _mapper;
        private readonly ICategoryService _categories;
        private readonly ITagService _tags;
        private readonly ILikeService _likeService;

        public MissionsController(IMissionService service, IMapper mapper, ICategoryService categories, ITagService tags, ILikeService likeService)
        {
            _service = service;
            _mapper = mapper;
            _categories = categories;
            _tags = tags;
            _likeService = likeService;
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Index(string? q, int? categoryId, int? tagId, int page = 1, int pageSize = 10)
        {
            var (items, total) = await _service.SearchPagedAsync(q, categoryId, tagId, page, pageSize);

            var vm = new MissionsIndexViewModel
            {
                Q = q,
                CategoryId = categoryId,
                TagId = tagId,
                Page = page,
                PageSize = pageSize,
                Items = items,
                TotalCount = total,
                Categories = (await _categories.GetAllAsync(1, int.MaxValue))
                    .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name }),
                Tags = (await _tags.GetAllAsync(1, int.MaxValue))
                    .Select(t => new SelectListItem { Value = t.Id.ToString(), Text = t.Name })

            };

            return View(vm);
        }
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            MissionDto? dto = await _service.GetByIdAsync(id);
            if (dto == null) return NotFound();

            var vm = _mapper.Map<MissionViewModel>(dto);

            var userId = User.Identity?.IsAuthenticated == true
                ? User.FindFirstValue(ClaimTypes.NameIdentifier)!
                : null;

            var isLiked = userId != null && await _likeService.IsMissionLikedByUserAsync(id, userId);
            var total = await _likeService.CountMissionLikesAsync(id);

            ViewBag.MissionLike = new LikeButtonViewModel
            {
                TargetType = LikeTargetType.Mission,
                TargetId = id,
                IsLikedByCurrentUser = isLiked,
                TotalCount = total,
                IsAuthenticated = User.Identity?.IsAuthenticated == true
            };

            return View(vm);
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(MissionViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);
            var dto = _mapper.Map<MissionDto>(vm);
            await _service.CreateAsync(dto);
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var dto = await _service.GetByIdAsync(id);
            if (dto == null) return NotFound();

            var vm = _mapper.Map<MissionViewModel>(dto);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MissionViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var dto = _mapper.Map<MissionDto>(vm);
            await _service.UpdateAsync(vm.Id, dto);
            return RedirectToAction(nameof(Details), new { id = vm.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var dto = await _service.GetByIdAsync(id);
            if (dto == null) return NotFound();

            var vm = _mapper.Map<MissionViewModel>(dto);
            return View(vm);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _service.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        
    }
}
    
