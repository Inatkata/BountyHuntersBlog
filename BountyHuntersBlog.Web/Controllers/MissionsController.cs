using BountyHuntersBlog.Models.Domain;
using BountyHuntersBlog.Models.ViewModels;
using BountyHuntersBlog.Repositories;
using BountyHuntersBlog.Web.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BountyHuntersBlog.Controllers
{
    public class MissionsController : Controller
    {
        private readonly IMissionPostRepository missionPostRepository;
        private readonly IMissionLikeRepository missionLikeRepository;
        private readonly SignInManager<Hunter> signInManager;
        private readonly UserManager<Hunter> userManager;
        private readonly IMissionCommentRepository missionCommentRepository;

        public MissionsController(
            IMissionPostRepository missionPostRepository,
            IMissionLikeRepository missionLikeRepository,
            SignInManager<Hunter> signInManager,
            UserManager<Hunter> userManager,
            IMissionCommentRepository missionCommentRepository)
        {
            this.missionPostRepository = missionPostRepository;
            this.missionLikeRepository = missionLikeRepository;
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.missionCommentRepository = missionCommentRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string urlHandle)
        {
            var liked = false;
            var missionPost = await missionPostRepository.GetByUrlHandleAsync(urlHandle);
            var missionDetailsViewModel = new MissionDetailsViewModel();

            if (missionPost != null)
            {
                var totalLikes = await missionLikeRepository.GetTotalLikesAsync(missionPost.Id);

                if (signInManager.IsSignedIn(User))
                {
                    var likes = await missionLikeRepository.GetLikesByMissionIdAsync(missionPost.Id);
                    var userId = userManager.GetUserId(User);

                    if (userId != null)
                    {
                        liked = likes.Any(x => x.UserId == userId);
                    }
                }

                // Get comments
                var missionComments = await missionCommentRepository.GetCommentsByMissionIdAsync(missionPost.Id);

                var viewComments = new List<MissionCommentViewModel>();
                foreach (var comment in missionComments)
                {
                    var username = (await userManager.FindByIdAsync(comment.UserId))?.UserName ?? "Unknown";
                    viewComments.Add(new MissionCommentViewModel
                    {
                        Description = comment.Description,
                        DateAdded = comment.DateAdded,
                        Username = username
                    });
                }

                missionDetailsViewModel = new MissionDetailsViewModel
                {
                    Id = missionPost.Id,
                    Heading = missionPost.Title,
                    PageTitle = missionPost.PageTitle,
                    Content = missionPost.Content,
                    ShortDescription = missionPost.ShortDescription,
                    UrlHandle = missionPost.UrlHandle,
                    FeaturedImageUrl = missionPost.FeaturedImageUrl,
                    MissionDate = missionPost.MissionDate,
                    Author = missionPost.Author?.UserName ?? "Unknown",
                    Visible = missionPost.Visible,
                    Tags = missionPost.Factions.ToList(),
                    TotalLikes = totalLikes,
                    Liked = liked,
                    Comments = viewComments
                };
            }

            return View(missionDetailsViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Index(MissionDetailsViewModel model)
        {
            if (signInManager.IsSignedIn(User))
            {
                var comment = new MissionComment
                {
                    MissionPostId = model.Id,
                    Description = model.CommentDescription,
                    UserId = userManager.GetUserId(User),
                    DateAdded = DateTime.Now
                };

                await missionCommentRepository.AddAsync(comment);

                return RedirectToAction("Index", "Missions", new { urlHandle = model.UrlHandle });
            }

            return View();
        }
    }
}
