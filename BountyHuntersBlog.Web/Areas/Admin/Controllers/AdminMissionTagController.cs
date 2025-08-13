// Areas/Admin/Controllers/AdminMissionTagsController.cs
using BountyHuntersBlog.Services.Interfaces;
using BountyHuntersBlog.ViewModels.Missions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class AdminMissionTagsController : Controller
{
    private readonly IMissionTagService _missionTags;
    private readonly IMissionService _missions;
    private readonly ITagService _tags;

    public AdminMissionTagsController(IMissionTagService missionTags, IMissionService missions, ITagService tags)
    {
        _missionTags = missionTags;
        _missions = missions;
        _tags = tags;
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int missionId)
    {
        var mission = await _missions.GetByIdAsync(missionId);
        if (mission == null) return NotFound();

        var allTags = (await _tags.AllAsync()).ToList();
        var selected = mission.TagIds.ToHashSet();

        var vm = new MissionEditViewModel
        {
            Id = mission.Id,
            Title = mission.Title,
            Description = mission.Description,
            CategoryId = mission.CategoryId,
            TagIds = mission.TagIds.ToList(),
            Tags = allTags.Select(t => new SelectListItem(text: t.Name, value: t.Id.ToString(), selected: selected.Contains(t.Id))).ToList()
        };

        return View(vm);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int missionId, int[] tagIds)
    {
        await _missionTags.UpdateTagsAsync(missionId, tagIds ?? Array.Empty<int>());
        TempData["Success"] = "Tags updated.";
        return RedirectToAction("Edit", "AdminMissions", new { area = "Admin", id = missionId });
    }
}