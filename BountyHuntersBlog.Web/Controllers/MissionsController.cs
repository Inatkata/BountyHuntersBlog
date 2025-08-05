using BountyHuntersBlog.Models.Domain;
using BountyHuntersBlog.Models.ViewModels;
using BountyHuntersBlog.Web.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BountyHuntersBlog.Web.Controllers
{
    public class MissionsController : Controller
    {
        private readonly IMissionPostRepository missionRepository;
        private readonly IMissionLikeRepository likeRepository;
        private readonly IMissionCommentRepository commentRepository;
        private readonly UserManager<IdentityUser> userManager;

        public MissionsController(
            IMissionPostRepository missionRepository,
            IMissionLikeRepository likeRepository,
            IMissionCommentRepository commentRepository,
            UserManager<IdentityUser> userManager)
        {
            this.missionRepository = missionRepository;
            this.likeRepository = likeRepository;
            this.commentRepository = commentRepository;
            this.userManager = userManager;
        }

        [HttpGet("{urlHandle}")]
        [HttpGet("{urlHandle}")]
        public async Task<IActionResult> Details(string urlHandle)
        {
            var mission = (await missionRepository.GetAllAsync())
                .FirstOrDefault(m => m.UrlHandle == urlHandle && m.Visible);

            if (mission == null)
                return View("NotFound");

            var likes = await likeRepository.GetTotalLikes(mission.Id);
            var userId = userManager.GetUserId(User);
            var liked = userId != null && await likeRepository.AlreadyLiked(mission.Id, userId);
            var comments = await commentRepository.GetAllAsync(mission.Id);
            var author = await userManager.FindByIdAsync(mission.AuthorId);

            var model = new MissionDetailsViewModel
            {
                Id = mission.Id,
                Title = mission.Title,
                PageTitle = mission.PageTitle,
                Content = mission.Content,
                FeaturedImageUrl = mission.FeaturedImageUrl,
                MissionDate = mission.MissionDate,
                UrlHandle = mission.UrlHandle,
                Factions = mission.Factions.ToList(),
                TotalLikes = likes,
                Liked = liked,
                Author = author as Hunter,
                Comments = comments.Select(c => new MissionComment
                {
                    Description = c.Description,
                    UserId = c.UserId,
                    DateAdded = c.DateAdded,
                    MissionPostId = c.MissionPostId
                }).ToList()
            };

            return View(model);
        }

    }
}
