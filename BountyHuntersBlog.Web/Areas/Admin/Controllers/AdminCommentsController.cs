// Areas/Admin/Controllers/AdminCommentsController.cs
using AutoMapper;
using BountyHuntersBlog.Services.Interfaces;
using BountyHuntersBlog.ViewModels.Admin.Comments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BountyHuntersBlog.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminCommentsController : Controller
    {
        private readonly ICommentService _comments;
        private readonly IMapper _mapper;

        public AdminCommentsController(ICommentService comments, IMapper mapper)
        {
            _comments = comments;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var list = (await _comments.AllAsync())
                .Select(_mapper.Map<AdminCommentListItemVM>)
                .OrderByDescending(x => x.CreatedOn)
                .ToList();
            return View(list);
        }

        // NEW: Details preview before delete / moderation
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var dto = await _comments.GetByIdAsync(id);
            if (dto == null) return NotFound();

            var vm = _mapper.Map<AdminCommentListItemVM>(dto);
            return View(vm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _comments.SoftDeleteAsync(id);
            TempData[result != null ? "Success" : "Error"] = result != null ? "Comment deleted." : "Comment not found.";
            return RedirectToAction(nameof(Index));
        }
    }
}