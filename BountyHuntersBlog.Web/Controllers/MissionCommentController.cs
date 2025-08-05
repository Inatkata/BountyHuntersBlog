using BountyHuntersBlog.Models.Domain;
using BountyHuntersBlog.Web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BountyHuntersBlog.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MissionCommentController : ControllerBase
    {
        private readonly IMissionCommentRepository commentRepository;

        public MissionCommentController(IMissionCommentRepository commentRepository)
        {
            this.commentRepository = commentRepository;
        }

        [HttpPost("Add")]
        [Authorize]
        public async Task<IActionResult> AddComment([FromBody] MissionComment request)
        {
            if (string.IsNullOrWhiteSpace(request.Description))
            {
                return BadRequest("Empty comment");
            }

            request.DateAdded = DateTime.UtcNow;

            await commentRepository.AddAsync(request);
            return Ok();
        }

        [HttpGet("{missionPostId}")]
        public async Task<IActionResult> GetCommentsForMission([FromRoute] Guid missionPostId)
        {
            var comments = await commentRepository.GetAllAsync(missionPostId);
            return Ok(comments);
        }
    }
}