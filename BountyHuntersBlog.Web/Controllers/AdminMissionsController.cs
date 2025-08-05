using BountyHuntersBlog.Models.Domain;
using BountyHuntersBlog.Models.ViewModels;
using BountyHuntersBlog.Repositories;
using BountyHuntersBlog.Web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BountyHuntersBlog.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminMissionsController : Controller
    {
        private readonly IMissionPostRepository missionPostRepository;
        private readonly IFactionRepository factionRepository;
        private readonly IImageRepository imageRepository;
        private readonly UserManager<IdentityUser> userManager;

        public AdminMissionsController(
            IMissionPostRepository missionPostRepository,
            IFactionRepository factionRepository,
            IImageRepository imageRepository,
            UserManager<IdentityUser> userManager)
        {
            this.missionPostRepository = missionPostRepository;
            this.factionRepository = factionRepository;
            this.imageRepository = imageRepository;
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var factions = await factionRepository.GetAllAsync();

            var model = new AddMissionPostRequest
            {
                Factions = factions.Select(f => new SelectListItem
                {
                    Text = f.DisplayName,
                    Value = f.Id.ToString()
                }).ToList()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddMissionPostRequest request)
        {
            var imageUrl = await imageRepository.UploadAsync(request.FeaturedImage);

            var mission = new MissionPost
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                PageTitle = request.PageTitle,
                Content = request.Content,
                ShortDescription = request.ShortDescription,
                FeaturedImageUrl = imageUrl,
                UrlHandle = request.UrlHandle,
                MissionDate = request.MissionDate,
                Status = request.Status,
                Visible = request.Visible,
                AuthorId = userManager.GetUserId(User)
            };

            await missionPostRepository.AddAsync(mission, request.SelectedFactions);

            return RedirectToAction("List");
        }
        [HttpGet]
        public async Task<IActionResult> List()
        {
            var missions = await missionPostRepository.GetAllAsync();
            return View(missions);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var mission = await missionPostRepository.GetAsync(id);
            var allFactions = await factionRepository.GetAllAsync();

            if (mission == null)
            {
                return NotFound();
            }

            var model = new EditMissionPostRequest
            {
                Id = mission.Id,
                Title = mission.Title,
                PageTitle = mission.PageTitle,
                ShortDescription = mission.ShortDescription,
                Content = mission.Content,
                FeaturedImageUrl = mission.FeaturedImageUrl,
                UrlHandle = mission.UrlHandle,
                MissionDate = mission.MissionDate,
                Status = mission.Status,
                Visible = mission.Visible,
                SelectedFactions = mission.Factions.Select(f => f.Id).ToList(),
                Factions = allFactions.Select(f => new SelectListItem
                {
                    Text = f.DisplayName,
                    Value = f.Id.ToString()
                }).ToList()
            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(EditMissionPostRequest request)
        {
            var existing = await missionPostRepository.GetAsync(request.Id);
            if (existing == null)
            {
                return NotFound();
            }

            // Качваме нова снимка, ако има
            string imageUrl = existing.FeaturedImageUrl;
            if (request.FeaturedImage != null)
            {
                imageUrl = await imageRepository.UploadAsync(request.FeaturedImage);
            }

            existing.Title = request.Title;
            existing.PageTitle = request.PageTitle;
            existing.ShortDescription = request.ShortDescription;
            existing.Content = request.Content;
            existing.FeaturedImageUrl = imageUrl;
            existing.UrlHandle = request.UrlHandle;
            existing.MissionDate = request.MissionDate;
            existing.Status = request.Status;
            existing.Visible = request.Visible;

            await missionPostRepository.UpdateAsync(existing, request.SelectedFactions);

            return RedirectToAction("List");
        }


    }
}
