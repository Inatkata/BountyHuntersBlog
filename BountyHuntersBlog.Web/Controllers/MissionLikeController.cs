using BountyHuntersBlog.Models.Domain;
using BountyHuntersBlog.Web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BountyHuntersBlog.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MissionLikeController : ControllerBase
    {
        private readonly IMissionLikeRepository missionLikeRepository;

        public MissionLikeController(IMissionLikeRepository missionLikeRepository)
        {
            this.missionLikeRepository = missionLikeRepository;
        }

        [HttpPost("Add")]
        [Authorize]
        public async Task<IActionResult> AddLike([FromBody] MissionLike request)
        {
            if (await missionLikeRepository.AlreadyLiked(request.MissionPostId, request.UserId))
            {
                return BadRequest("Already liked");
            }

            await missionLikeRepository.AddLike(request);
            return Ok();
        }

        [HttpGet("{missionPostId}/totalLikes")]
        public async Task<IActionResult> GetTotalLikes([FromRoute] Guid missionPostId)
        {
            var totalLikes = await missionLikeRepository.GetTotalLikes(missionPostId);
            return Ok(totalLikes);
        }
    }
}