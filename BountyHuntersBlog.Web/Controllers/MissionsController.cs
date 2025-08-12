using AutoMapper;
using BountyHuntersBlog.Services.DTOs;
using BountyHuntersBlog.Services.Interfaces;
using BountyHuntersBlog.ViewModels.Admin.Missions;
using BountyHuntersBlog.ViewModels.Missions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using BountyHuntersBlog.ViewModels;

namespace BountyHuntersBlog.Web.Controllers
{
    public class MissionsController : Controller
    {
        private readonly IMissionService _missions;
        private readonly ICategoryService _categories;
        private readonly ITagService _tags;
        private readonly ICommentService _comments;
        private readonly ILikeService _likes;
        private readonly IMapper _mapper;

        public MissionsController(
            IMissionService missions,
            ICategoryService categories,
            ITagService tags,
            ICommentService comments,
            ILikeService likes,
            IMapper mapper)
        {
            _missions = missions;
            _categories = categories;
            _tags = tags;
            _comments = comments;
            _likes = likes;
            _mapper = mapper;
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
                Items = items.Select(_mapper.Map<MissionListItemViewModel>).ToList(),
                Categories = (await _categories.AllAsync())
                    .Select(c => new SelectListItem(c.Name, c.Id.ToString())),
                Tags = (await _tags.AllAsync())
                    .Select(t => new SelectListItem(t.Name, t.Id.ToString()))
            };

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var dto = await _missions.GetDetailsAsync(id);
            if (dto == null) return NotFound();

            var vm = _mapper.Map<MissionDetailsViewModel>(dto);

            // Зареждаме коментари
            var comments = await _comments.GetForMissionAsync(id);
            vm.Comments = comments.Select(c => new MissionDetailsViewModel.CommentItem
            {
                Id = c.Id,
                Content = c.Content,
                CreatedOn = c.CreatedOn,
                UserId = c.UserId,
                UserDisplayName = c.UserDisplayName,
                CanDelete = User.Identity?.IsAuthenticated == true &&
                            (User.IsInRole("Admin") || c.UserDisplayName == User.Identity!.Name)
            }).ToList();

            return View(vm);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var vm = new AdminMissionFormVM
            {
                Categories = (await _categories.AllAsync())
                    .Select(c => new SelectListItem(c.Name, c.Id.ToString())),
                Tags = (await _tags.AllAsync())
                    .Select(t => new SelectListItem(t.Name, t.Id.ToString()))
            };
            return View(vm);
        }

        [Authorize]
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
            return RedirectToAction(nameof(Details), new { id });
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var dto = await _missions.GetByIdAsync(id);
            if (dto == null) return NotFound();

            // проверка за автор/админ
            if (!User.IsInRole("Admin") && dto.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
                return Forbid();

            var vm = new MissionEditViewModel
            {
                Id = dto.Id,
                Title = dto.Title,
                Description = dto.Description,
                CategoryId = dto.CategoryId,
                TagIds = dto.TagIds.ToList(),
                Categories = (await _categories.AllAsync())
                    .Select(c => new SelectListItem(c.Name, c.Id.ToString())),
                Tags = (await _tags.AllAsync())
                    .Select(t => new SelectListItem(t.Name, t.Id.ToString()))
            };

            return View(vm);
        }

        [Authorize]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MissionEditViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Categories = (await _categories.AllAsync()).Select(c => new SelectListItem(c.Name, c.Id.ToString()));
                vm.Tags = (await _tags.AllAsync()).Select(t => new SelectListItem(t.Name, t.Id.ToString()));
                return View(vm);
            }

            var dto = await _missions.GetByIdAsync(vm.Id);
            if (dto == null) return NotFound();

            // проверка за автор/админ
            if (!User.IsInRole("Admin") && dto.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
                return Forbid();

            await _missions.UpdateAsync(new MissionDto
            {
                Id = vm.Id,
                Title = vm.Title,
                Description = vm.Description,
                CategoryId = vm.CategoryId,
                TagIds = vm.TagIds
            });

            return RedirectToAction(nameof(Details), new { id = vm.Id });
        }

        [Authorize]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var dto = await _missions.GetByIdAsync(id);
            if (dto == null) return NotFound();

            if (!User.IsInRole("Admin") && dto.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
                return Forbid();

            await _missions.SoftDeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment(CommentCreateViewModel vm)
        {
            if (!ModelState.IsValid)
                return RedirectToAction(nameof(Details), new { id = vm.MissionId });

            await _comments.AddAsync(
                vm.MissionId,
                User.FindFirstValue(ClaimTypes.NameIdentifier)!,
                vm.Content
            );


            return RedirectToAction(nameof(Details), new { id = vm.MissionId });
        }

        [Authorize]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteComment(int id, int missionId)
        {
            var comment = await _comments.GetByIdAsync(id);
            if (comment == null) return NotFound();

            if (!User.IsInRole("Admin") && comment.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
                return Forbid();

            await _comments.DeleteAsync(id);
            return RedirectToAction(nameof(Details), new { id = missionId });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ToggleMissionLike(int id)
        {
            await _likes.ToggleMissionLikeAsync(id, User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            return RedirectToAction(nameof(Details), new { id });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ToggleCommentLike(int id, int missionId)
        {
            await _likes.ToggleCommentLikeAsync(id, User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            return RedirectToAction(nameof(Details), new { id = missionId });
        }
    }
}
