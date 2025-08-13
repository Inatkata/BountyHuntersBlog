using BountyHuntersBlog.Services.DTOs;
using BountyHuntersBlog.Services.Interfaces;
using BountyHuntersBlog.ViewModels.Comments;
using BountyHuntersBlog.ViewModels.Missions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AddCommentRequest = BountyHuntersBlog.ViewModels.Comments.AddCommentRequest;

namespace BountyHuntersBlog.Web.Controllers
{
    public class MissionsController : Controller
    {
        private readonly IMissionService _service;

        public MissionsController(IMissionService service)
            => _service = service;

        [HttpGet]
        public async Task<IActionResult> Index(string? q, int? categoryId, int? tagId, int page = 1, int pageSize = 10)
        {
            var (items, total) = await _service.SearchPagedAsync(q, categoryId, tagId, page, pageSize);

            var vm = new MissionIndexViewModel
            {
                Q = q,
                CategoryId = categoryId,
                TagId = tagId,
                Page = page,
                PageSize = pageSize,
                TotalCount = total,
                Items = items.Select(m => new MissionListItemVm
                {
                    Id = m.Id,
                    Title = m.Title,
                    IsCompleted = m.IsCompleted,
                    CategoryName = m.CategoryName,
                    TagNames = m.TagNames
                }).ToList(),
                Categories = await _service.GetCategoriesSelectListAsync(),
                Tags = await _service.GetTagsSelectListAsync()
            };

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var dto = await _service.GetDetailsAsync(id);
            if (dto == null) return NotFound();

            var vm = new MissionDetailsViewModel
            {
                Id = dto.Id,
                Title = dto.Title,
                Description = dto.Description,
                ImageUrl = dto.ImageUrl,
                CategoryName = dto.CategoryName,
                TagNames = dto.TagNames,
                LikesCount = dto.LikesCount,
                Comments = dto.Comments.Select(c => new MissionCommentVm
                {
                    Id = c.Id,
                    Content = c.Content,
                    CreatedOn = c.CreatedOn,
                    UserDisplayName = c.UserDisplayName,
                    CanDelete = c.CanDelete
                }).ToList()
            };

            return View(vm);
        }

        [Authorize, HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleMissionLike(int id)
        {
            await _service.ToggleMissionLikeAsync(id, User);
            return RedirectToAction(nameof(Details), new { id });
        }

        [Authorize, HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment(AddCommentRequest model)
        {
            if (!ModelState.IsValid)
                return RedirectToAction(nameof(Details), new { id = model.MissionId });

            await _service.AddCommentAsync(model.MissionId, model.Content, User);
            return RedirectToAction(nameof(Details), new { id = model.MissionId });
        }

        [Authorize, HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleCommentLike(int id, int missionId)
        {
            await _service.ToggleCommentLikeAsync(id, User);
            return RedirectToAction(nameof(Details), new { id = missionId });
        }

        [Authorize, HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteComment(int id, int missionId)
        {
            await _service.DeleteCommentAsync(id, User);
            return RedirectToAction(nameof(Details), new { id = missionId });
        }

        [Authorize, HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var dto = await _service.GetEditAsync(id);
            if (dto == null) return NotFound();

            var vm = new MissionEditViewModel
            {
                Id = dto.Id,
                Title = dto.Title,
                Description = dto.Description,
                ImageUrl = dto.ImageUrl,
                CategoryId = dto.CategoryId,
                TagIds = dto.TagIds.ToList(),
                IsCompleted = dto.IsCompleted,
                Categories = await _service.GetCategoriesSelectListAsync(),
                Tags = await _service.GetTagsSelectListAsync()
            };
            return View(vm);
        }

        [Authorize, HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MissionEditViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Categories = await _service.GetCategoriesSelectListAsync();
                vm.Tags = await _service.GetTagsSelectListAsync();
                return View(vm);
            }

            await _service.EditAsync(new()
            {
                Id = vm.Id,
                Title = vm.Title,
                Description = vm.Description,
                ImageUrl = vm.ImageUrl,
                CategoryId = vm.CategoryId,
                TagIds = vm.TagIds ?? new List<int>(),
                IsCompleted = vm.IsCompleted
            });

            return RedirectToAction(nameof(Details), new { id = vm.Id });
        }
        [Authorize, HttpGet]
        public async Task<IActionResult> Create()
        {
            var vm = new MissionEditViewModel
            {
                Categories = await _service.GetCategoriesSelectListAsync(),
                Tags = await _service.GetTagsSelectListAsync()
            };
            return View(vm);
        }

        [Authorize, HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MissionEditViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Categories = await _service.GetCategoriesSelectListAsync();
                vm.Tags = await _service.GetTagsSelectListAsync();
                return View(vm);
            }

            // ако имаш метод CreateAsync в сервиса
            await _service.CreateAsync(new MissionEditDto
            {
                Title = vm.Title,
                Description = vm.Description,
                ImageUrl = vm.ImageUrl,
                CategoryId = vm.CategoryId,
                TagIds = vm.TagIds ?? new List<int>(),
                IsCompleted = vm.IsCompleted
            });

            return RedirectToAction(nameof(Index));
        }

    }
}
