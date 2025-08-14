using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using BountyHuntersBlog.Services.Interfaces;

namespace BountyHuntersBlog.Web.Controllers
{
    public class LikesController : BaseController
    {
        private readonly ILikeService _likes;
        public LikesController(ILikeService likes) => _likes = likes;

        [Authorize]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleMission(int missionId)
        {
            var userId = GetUserId()!;
            var result = await _likes.ToggleMissionLikeAsync(missionId, userId);
            return Json(result);
        }

        [Authorize]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleComment(int commentId)
        {
            var userId = GetUserId()!;
            var result = await _likes.ToggleCommentLikeAsync(commentId, userId);
            return Json(result);
        }
    }
}