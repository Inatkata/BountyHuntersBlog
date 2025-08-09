using System.Security.Claims;
using BountyHuntersBlog.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BountyHuntersBlog.Web.Controllers
{
    [Authorize]
    public class LikesController : Controller
    {
        private readonly ILikeService _service;

        public LikesController(ILikeService service) => _service = service;

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleMission(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var result = await _service.ToggleMissionLikeAsync(id, userId);
            return Json(new { liked = result.IsLiked, count = result.TotalCount });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleComment(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var result = await _service.ToggleCommentLikeAsync(id, userId);
            return Json(new { liked = result.IsLiked, count = result.TotalCount });
        }
    }
}