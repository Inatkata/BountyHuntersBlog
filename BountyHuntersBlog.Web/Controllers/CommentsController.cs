// BountyHuntersBlog.Web/Controllers/CommentsController.cs
using System.Security.Claims;
using AutoMapper;
using BountyHuntersBlog.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

public class CommentsController : Controller
{
    private readonly ICommentService _comments;
    private readonly IMapper _mapper;

    public CommentsController(ICommentService comments, IMapper mapper)
    {
        _comments = comments;
        _mapper = mapper;
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(int missionId, string content)
    {
        var userId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier)!;
        var dto = await _comments.AddAsync(missionId, userId, content);
        return RedirectToAction("Details", "Missions", new { id = missionId });
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, int missionId)
    {
        await _comments.DeleteAsync(id);
        return RedirectToAction("Details", "Missions", new { id = missionId });
    }
}