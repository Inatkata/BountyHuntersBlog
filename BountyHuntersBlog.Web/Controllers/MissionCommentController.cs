using BountyHuntersBlog.Models.Domain;
using BountyHuntersBlog.Models.ViewModels;
using BountyHuntersBlog.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BountyHuntersBlog.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class MissionCommentController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IMissionCommentRepository missionCommentRepository;

        public MissionCommentController(
            UserManager<ApplicationUser> userManager,
            IMissionCommentRepository missionCommentRepository)
        {
            this.userManager = userManager;
            this.missionCommentRepository = missionCommentRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddCommentRequest request)
        {
            var userId = userManager.GetUserId(User);

            var comment = new MissionComment
            {
                MissionPostId = request.MissionPostId,
                Description = request.Description,
                DateAdded = DateTime.UtcNow,
                ApplicationUserId = userId
            };

            await missionCommentRepository.AddAsync(comment);
            return Ok();
        }
    }
}