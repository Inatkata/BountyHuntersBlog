using BountyHuntersBlog.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BountyHuntersBlog.Controllers
{
    [Authorize]
    public class MissionLikeController : Controller
    {
        private readonly IMissionLikeRepository missionLikeRepository;

        public MissionLikeController(IMissionLikeRepository missionLikeRepository)
        {
            this.missionLikeRepository = missionLikeRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Like(Guid missionPostId, Guid hunterId)
        {
            await missionLikeRepository.AddLikeAsync(missionPostId, hunterId);
            var totalLikes = await missionLikeRepository.GetTotalLikes(missionPostId);

            return Json(new { totalLikes });
        }
    }
}