// BountyHuntersBlog.Web/Controllers/LikesController.cs
using System.Security.Claims;
using BountyHuntersBlog.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

public class LikesController : Controller
{
    private readonly ILikeService _likes;
    public LikesController(ILikeService likes) => _likes = likes;

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> ToggleMission(int missionId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var result = await _likes.ToggleMissionLikeAsync(missionId, userId);
        return Json(result);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> ToggleComment(int commentId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var result = await _likes.ToggleCommentLikeAsync(commentId, userId);
        return Json(result);
    }
}