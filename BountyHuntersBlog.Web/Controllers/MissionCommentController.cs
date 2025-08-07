using BountyHuntersBlog.Models.Domain;
using BountyHuntersBlog.Models.ViewModels;
using BountyHuntersBlog.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BountyHuntersBlog.Controllers
{
    [Authorize]
    public class MissionCommentController : Controller
    {
        private readonly IMissionCommentRepository commentRepository;

        public MissionCommentController(IMissionCommentRepository commentRepository)
        {
            this.commentRepository = commentRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddMissionCommentRequest request)
        {
            if (ModelState.IsValid)
            {
                var comment = new MissionComment
                {
                    Id = Guid.NewGuid(),
                    Content = request.Content,
                    CreatedAt = DateTime.UtcNow,
                    MissionPostId = request.MissionPostId,
                    HunterId = request.HunterId
                };

                await commentRepository.AddAsync(comment);
            }

            return RedirectToAction("Details", "Mission", new { id = request.MissionPostId });
        }
    }
}