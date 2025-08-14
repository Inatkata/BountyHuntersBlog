using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BountyHuntersBlog.Services.Interfaces;
using BountyHuntersBlog.Services.DTOs;

namespace BountyHuntersBlog.Web.Controllers
{
    public class CommentsController : BaseController
    {
        private readonly ICommentService _comments;

        public CommentsController(ICommentService comments) => _comments = comments;

        [Authorize]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int missionId, string content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                TempData["Error"] = "Comment cannot be empty.";
                return RedirectToAction("Details", "Missions", new { id = missionId });
            }

            await _comments.CreateAsync(new CommentDto { MissionId = missionId, Content = content });
            TempData["Success"] = "Comment added.";
            return RedirectToAction("Details", "Missions", new { id = missionId });
        }

        [Authorize]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, int missionId)
        {
            await _comments.SoftDeleteAsync(id);
            TempData["Success"] = "Comment deleted.";
            return RedirectToAction("Details", "Missions", new { id = missionId });
        }
    }
}