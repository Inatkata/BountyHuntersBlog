using BountyHuntersBlog.Services.Interfaces;
using BountyHuntersBlog.ViewModels.Comments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BountyHuntersBlog.Web.Controllers
{
    [Authorize]
    public class CommentsController : Controller
    {
        private readonly ICommentService _comments;
        private readonly ILikeService _likes;

        public CommentsController(ICommentService comments, ILikeService likes)
        {
            _comments = comments;
            _likes = likes;
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var comment = await _comments.GetByIdAsync(id);
            if (comment == null) return NotFound();

            // Автор или админ
            if (!User.IsInRole("Admin") && comment.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
                return Forbid();

            var vm = new CommentViewModel
            {
                Id = comment.Id,
                Content = comment.Content,
                MissionId = comment.MissionId,
                CreatedOn = comment.CreatedOn,
                UserName = comment.UserDisplayName
            };

            return View(vm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CommentViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var comment = await _comments.GetByIdAsync(vm.Id);
            if (comment == null) return NotFound();

            if (!User.IsInRole("Admin") && comment.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
                return Forbid();

            await _comments.EditAsync(vm.Id, vm.Content);
            return RedirectToAction("Details", "Missions", new { id = vm.MissionId });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, int missionId)
        {
            var comment = await _comments.GetByIdAsync(id);
            if (comment == null) return NotFound();

            if (!User.IsInRole("Admin") && comment.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
                return Forbid();

            await _comments.DeleteAsync(id);
            return RedirectToAction("Details", "Missions", new { id = missionId });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleLike(int id, int missionId)
        {
            await _likes.ToggleCommentLikeAsync(id, User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            return RedirectToAction("Details", "Missions", new { id = missionId });
        }
    }
}
