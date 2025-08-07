using BountyHuntersBlog.Data;
using BountyHuntersBlog.Models.Domain;
using BountyHuntersBlog.Models.ViewModels;
using BountyHuntersBlog.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BountyHuntersBlog.Controllers
{
    public class MissionsController : Controller
    {
        private readonly BountyHuntersDbContext dbContext;
        private readonly IMissionPostRepository missionPostRepository;
        private readonly IMissionLikeRepository missionLikeRepository;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IMissionCommentRepository missionCommentRepository;

        public MissionsController(
            BountyHuntersDbContext dbContext,
            IMissionPostRepository missionPostRepository,
            IMissionLikeRepository missionLikeRepository,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IMissionCommentRepository missionCommentRepository)
        {
            this.dbContext = dbContext;
            this.missionPostRepository = missionPostRepository;
            this.missionLikeRepository = missionLikeRepository;
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.missionCommentRepository = missionCommentRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var missions = await missionPostRepository.GetAllAsync();
            return View("Index", missions);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Like(Guid id)
        {
            var userId = userManager.GetUserId(User);
            var ApplicationUserGuid = Guid.Parse(userId);

            var alreadyLiked = await dbContext.MissionLikes
                .AnyAsync(x => x.MissionPostId == id && x.ApplicationUserId == userId);

            if (!alreadyLiked)
            {
                await dbContext.MissionLikes.AddAsync(new MissionLike
                {
                    MissionPostId = id,
                    ApplicationUserId = userId
                });

                await dbContext.SaveChangesAsync();
            }
            else
            {
                var existingLike = await dbContext.MissionLikes
                    .FirstOrDefaultAsync(x => x.MissionPostId == id && x.ApplicationUserId == userId);

                if (existingLike != null)
                {
                    dbContext.MissionLikes.Remove(existingLike);
                    await dbContext.SaveChangesAsync();
                }
            }

            var mission = await missionPostRepository.GetByIdAsync(id);
            return RedirectToAction("Details", new { urlHandle = mission.UrlHandle });
        }

        [HttpGet]
        public async Task<IActionResult> Details(string urlHandle)
        {
            var liked = false;
            var missionPost = await missionPostRepository.GetByUrlHandleAsync(urlHandle);

            if (missionPost == null) return View("Details", null);

            var comments = await missionCommentRepository.GetCommentsByMissionIdAsync(missionPost.Id);

            if (signInManager.IsSignedIn(User))
            {
                var userId = userManager.GetUserId(User);
                liked = await missionLikeRepository.AlreadyLiked(missionPost.Id, userId);
            }

            var viewModel = new MissionDetailsViewModel
            {
                Id = missionPost.Id,
                Title = missionPost.Title,
                PageTitle = missionPost.PageTitle,
                Content = missionPost.Content,
                FeaturedImageUrl = missionPost.FeaturedImageUrl,
                ShortDescription = missionPost.ShortDescription,
                MissionDate = missionPost.MissionDate,
                UrlHandle = missionPost.UrlHandle,
                User = missionPost.PostedByUser,
                Visible = missionPost.Visible,
                Liked = liked,
                Comments = comments.Select(x => new MissionCommentViewModel
                {
                    Description = x.Description,
                    DateAdded = x.DateAdded,
                    UserName = x.ApplicationUser?.DisplayName ?? "Unknown"
                }).ToList()
            };

            return View("Details", viewModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Details(MissionDetailsViewModel model)
        {
            var userId = userManager.GetUserId(User);

            var comment = new MissionComment
            {
                MissionPostId = model.Id,
                Description = model.CommentDescription,
                DateAdded = DateTime.UtcNow,
                ApplicationUserId = userId
            };

            await missionCommentRepository.AddAsync(comment);

            return RedirectToAction("Details", new { urlHandle = model.UrlHandle });
        }
    }
}
