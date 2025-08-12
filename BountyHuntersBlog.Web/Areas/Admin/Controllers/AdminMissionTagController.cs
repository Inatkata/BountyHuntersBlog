using BountyHuntersBlog.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BountyHuntersBlog.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminMissionTagsController : Controller
    {
        private readonly IMissionTagService _missionTags;
        private readonly IMissionService _missions;
        private readonly ITagService _tags;

        public AdminMissionTagsController(
            IMissionTagService missionTags,
            IMissionService missions,
            ITagService tags)
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

            var allTags = await _tags.AllAsync();
            var selected = mission.TagIds.ToHashSet();
            
            ViewBag.MissionId = missionId;
            ViewBag.MissionTitle = mission.Title;
            ViewBag.Tags = allTags.Select(t => new SelectListItem(t.Name, t.Id.ToString(), selected.Contains(t.Id))).ToList();

            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int missionId, int[] tagIds)
        {
            await _missionTags.UpdateTagsAsync(missionId, tagIds ?? Array.Empty<int>());
            TempData["Success"] = "Tags updated.";
            return RedirectToAction("Edit", "AdminMissions", new { id = missionId });
        }
    }
}