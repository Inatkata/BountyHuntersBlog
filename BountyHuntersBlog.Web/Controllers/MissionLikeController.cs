using BountyHuntersBlog.Models.Domain;
using BountyHuntersBlog.Models.ViewModels;
using BountyHuntersBlog.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BountyHuntersBlog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MissionLikeController : ControllerBase
    {
        private readonly IMissionLikeRepository missionLikeRepository;

        public MissionLikeController(IMissionLikeRepository missionLikeRepository)
        {
            this.missionLikeRepository = missionLikeRepository;
        }

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> AddLike([FromBody] AddMissionLikeRequest request)
        {
            var model = new MissionLike
            {
                MissionPostId = request.MissionPostId,
                UserId = request.UserId
            };

            await missionLikeRepository.AddLikeAsync(model);
            return Ok();
        }

        [HttpGet]
        [Route("{missionPostId:Guid}/totalLikes")]
        public async Task<IActionResult> GetTotalLikes([FromRoute] Guid missionPostId)
        {
            var totalLikes = await missionLikeRepository.GetTotalLikesAsync(missionPostId);
            return Ok(totalLikes);
        }
    }
}