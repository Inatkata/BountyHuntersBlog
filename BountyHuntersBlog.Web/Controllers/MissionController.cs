using BountyHuntersBlog.Models.ViewModels;
using BountyHuntersBlog.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BountyHuntersBlog.Controllers
{
    public class MissionController : Controller
    {
        private readonly IMissionPostRepository missionPostRepository;
        private readonly IMissionCommentRepository commentRepository;
        private readonly IMissionLikeRepository likeRepository;

        public MissionController(
            IMissionPostRepository missionPostRepository,
            IMissionCommentRepository commentRepository,
            IMissionLikeRepository likeRepository)
        {
            this.missionPostRepository = missionPostRepository;
            this.commentRepository = commentRepository;
            this.likeRepository = likeRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var mission = await missionPostRepository.GetAsync(id);
            if (mission == null) return NotFound();

            var likeCount = await likeRepository.GetTotalLikes(id);

            var model = new MissionDetailsViewModel
            {
                Id = mission.Id,
                Title = mission.Title,
                Content = mission.Content,
                MissionDate = mission.MissionDate,
                FeaturedImageUrl = mission.FeaturedImageUrl,
                AuthorUsername = mission.Author?.UserName,
                Comments = mission.Comments.ToList(),
                LikeCount = likeCount,
                CurrentHunterId = Guid.Empty
            };

            return View(model);
        }

    }
}