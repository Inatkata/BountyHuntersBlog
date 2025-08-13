using BountyHuntersBlog.Data.Models;
using BountyHuntersBlog.Services.DTOs;
using BountyHuntersBlog.Services.Interfaces;
using BountyHuntersBlog.ViewModels.Missions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BountyHuntersBlog.Web.Controllers
{
    public class MissionsController : Controller
    {
        private readonly IMissionService _missions;
        private readonly ICommentService _comments;
        private readonly ILikeService _likes;
        private readonly UserManager<ApplicationUser> _userManager; // ADD

        public MissionsController(
            IMissionService missions,
            ICommentService comments,
            ILikeService likes,
            UserManager<ApplicationUser> userManager) // ADD
        {
            _missions = missions;
            _comments = comments;
            _likes = likes;
            _userManager = userManager; // ADD
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var dto = await _missions.GetDetailsAsync(id);
            if (dto == null) return NotFound();

            var vm = MissionDetailsVM.FromDto(dto); // виж т.2 долу
            return View(vm);
        }

        // ===== Create =====
        [Authorize(Roles = "User,Administrator")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _missions.GetCategoriesSelectListAsync();
            ViewBag.Tags = await _missions.GetTagsSelectListAsync();
            return View(new MissionEditDto());
        }

        [Authorize(Roles = "User,Administrator")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MissionEditDto dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _missions.GetCategoriesSelectListAsync();
                ViewBag.Tags = await _missions.GetTagsSelectListAsync();
                return View(dto);
            }

            await _missions.CreateAsync(dto);
            return RedirectToAction("Index", "Home");
        }

        // ===== Edit =====
        [Authorize(Roles = "User,Administrator")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var edit = await _missions.GetEditAsync(id);
            if (edit == null) return NotFound();

            ViewBag.Categories = await _missions.GetCategoriesSelectListAsync();
            ViewBag.Tags = await _missions.GetTagsSelectListAsync();
            return View(edit);
        }

        [Authorize(Roles = "User,Administrator")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MissionEditDto dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _missions.GetCategoriesSelectListAsync();
                ViewBag.Tags = await _missions.GetTagsSelectListAsync();
                return View(dto);
            }

            await _missions.EditAsync(dto);
            return RedirectToAction(nameof(Details), new { id = dto.Id });
        }

        // ===== Delete =====
        [Authorize(Roles = "User,Administrator")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _missions.SoftDeleteAsync(id);
            return RedirectToAction("Index", "Home");
        }

        // ===== Comments =====
        [Authorize(Roles = "User,Administrator")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment(int missionId, string content)
        {
            await _missions.AddCommentAsync(missionId, content, User);
            return RedirectToAction(nameof(Details), new { id = missionId });
        }

        [Authorize(Roles = "User,Administrator")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteComment(int id, int missionId)
        {
            try
            {
                await _missions.DeleteCommentAsync(id, User);
                TempData["Success"] = "Comment deleted.";
                return RedirectToAction(nameof(Details), new { id = missionId });
            }
            catch (UnauthorizedAccessException)
            {
                Response.StatusCode = StatusCodes.Status403Forbidden;
                ViewBag.MissionId = missionId;
                return View("~/Views/Error/Forbidden.cshtml"); // HTML view, не JSON
            }
            catch (KeyNotFoundException)
            {
                return RedirectToAction("NotFoundPage", "Error");
            }
        }



        [Authorize(Roles = "User,Administrator")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleLike(int missionId)
        {
            var userId = _userManager.GetUserId(User)!; 
            var res = await _likes.ToggleMissionLikeAsync(missionId, userId);
            return Json(new { liked = res.IsLiked, total = res.LikesCount });
        }
    }
}
