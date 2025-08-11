using Microsoft.AspNetCore.Userization;
using Microsoft.AspNetCore.Mvc;
using BountyHuntersBlog.Services.Interfaces;

namespace BountyHuntersBlog.Web.Controllers
{
    [Userize]
    public class CommentsController : Controller
    {
        private readonly ICommentService _comments;

        public CommentsController(ICommentService comments) => _comments = comments;

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(int missionId, string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return RedirectToAction("Details", "Missions", new { id = missionId });

            await _comments.AddAsync(missionId, text, User);
            return RedirectToAction("Details", "Missions", new { id = missionId });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, int missionId)
        {
            await _comments.DeleteAsync(id, User);
            return RedirectToAction("Details", "Missions", new { id = missionId });
        }
    }
}