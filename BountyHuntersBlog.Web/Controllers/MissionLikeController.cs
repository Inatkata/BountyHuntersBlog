using BountyHuntersBlog.Models.Domain;
using BountyHuntersBlog.Models.Requests;
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
    public class MissionLikeController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IMissionLikeRepository missionLikeRepository;

        public MissionLikeController(
            UserManager<ApplicationUser> userManager,
            IMissionLikeRepository missionLikeRepository)
        {
            this.userManager = userManager;
            this.missionLikeRepository = missionLikeRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Like([FromBody] AddLikeRequest request)
        {
            var userId = userManager.GetUserId(User);

            var result = await missionLikeRepository.AddLike(request.MissionPostId, userId);

            if (result)
                return Ok();

            return BadRequest();
        }
    }
}