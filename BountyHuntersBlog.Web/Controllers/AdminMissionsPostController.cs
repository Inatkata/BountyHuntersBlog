using BountyHuntersBlog.Models.Domain;
using BountyHuntersBlog.Models.Requests;
using BountyHuntersBlog.Models.ViewModels;
using BountyHuntersBlog.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BountyHuntersBlog.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminMissionPostsController : Controller
    {
        private readonly IMissionPostRepository missionPostRepository;
        private readonly IFactionRepository factionRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public AdminMissionPostsController(
            IMissionPostRepository missionPostRepository,
            IFactionRepository factionRepository,
            UserManager<ApplicationUser> userManager)
        {
            this.missionPostRepository = missionPostRepository;
            this.factionRepository = factionRepository;
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
                    Text = f.Name,
                    Value = f.Id.ToString()
                }).ToList()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddMissionPostRequest model)
        {
            var applicationUser = await userManager.GetUserAsync(User);

            var mission = new MissionPost
            {
                Title = model.Title,
                PageTitle = model.PageTitle,
                Content = model.Content,
                ShortDescription = model.ShortDescription,
                FeaturedImageUrl = model.FeaturedImageUrl,
                UrlHandle = model.UrlHandle,
                MissionDate = model.MissionDate,
                Visible = model.Visible,
                FactionId = model.FactionId,
                PostedByUserId = applicationUser?.Id
            };

            await missionPostRepository.AddAsync(mission);
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
            var mission = await missionPostRepository.GetByIdAsync(id);
            var factions = await factionRepository.GetAllAsync();

            if (mission == null) return NotFound();

            var model = new EditMissionPostRequest
            {
                Id = mission.Id,
                Title = mission.Title,
                PageTitle = mission.PageTitle,
                Content = mission.Content,
                ShortDescription = mission.ShortDescription,
                FeaturedImageUrl = mission.FeaturedImageUrl,
                UrlHandle = mission.UrlHandle,
                MissionDate = mission.MissionDate,
                Visible = mission.Visible,
                FactionId = mission.FactionId,
                Factions = factions.Select(f => new SelectListItem
                {
                    Text = f.Name,
                    Value = f.Id.ToString()
                }).ToList()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditMissionPostRequest model)
        {
            var mission = new MissionPost
            {
                Id = model.Id,
                Title = model.Title,
                PageTitle = model.PageTitle,
                Content = model.Content,
                ShortDescription = model.ShortDescription,
                FeaturedImageUrl = model.FeaturedImageUrl,
                UrlHandle = model.UrlHandle,
                MissionDate = model.MissionDate,
                Visible = model.Visible,
                FactionId = model.FactionId
            };

            var updated = await missionPostRepository.UpdateAsync(mission);
            if (updated != null)
            {
                return RedirectToAction("List");
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(EditMissionPostRequest model)
        {
            var deleted = await missionPostRepository.DeleteAsync(model.Id);
            if (deleted != null)
            {
                return RedirectToAction("List");
            }

            return View("Edit", model);
        }
    }
}
