using BountyHuntersBlog.Models.Domain;
using BountyHuntersBlog.Models.Requests;
using BountyHuntersBlog.Repositories;
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
        public async Task<IActionResult> AddLike([FromBody] AddLikeRequest addLikeRequest)
        {
            var model = new MissionLike
            {
                MissionPostId = addLikeRequest.MissionPostId,
                HunterId = addLikeRequest.HunterId
            };

            await missionLikeRepository.AddLike(model);

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