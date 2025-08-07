using Azure.Core;
using BountyHuntersBlog.Models.Domain;
using BountyHuntersBlog.Models.Requests;
using BountyHuntersBlog.Repositories;
using BountyHuntersBlog.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BountyHuntersBlog.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminMissionPostsController : Controller
    {
        private readonly IFactionRepository factionRepository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IMissionService missionService;

        public AdminMissionPostsController(
            IFactionRepository factionRepository,
            UserManager<ApplicationUser> userManager,
            IMissionService missionService)
        {
            this.factionRepository = factionRepository;
            this.userManager = userManager;
            this.missionService = missionService;
        }


        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var factions = await factionRepository.GetAllAsync(null, null, null, 1, 100);

            var model = new AddMissionPostRequest
            {
                Factions = factions
                    .Select(f => new SelectListItem
                    {
                        Text = f.Name,
                        Value = f.Id.ToString()
                    })
                    .ToList()

            };

            return View(model);
        }

     
        [HttpPost]
        public async Task<IActionResult> Add(AddMissionPostRequest request)
        {
            var userId = userManager.GetUserId(User);
            await missionService.AddAsync(request, Guid.Parse(userId));
            return RedirectToAction("Add");
        }


        [HttpGet]
        public async Task<IActionResult> List()
        {
            var missions = await missionService.GetAllAsync();
            return View(missions);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var mission = await missionService.GetByIdAsync(id);
            var factions = await factionRepository.GetAllAsync(null, null, null, 1, 100);
        

            if (mission == null) return View(null);

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
                UserId = mission.PostedByUserId,
                Visible = mission.Visible,
                Status = mission.Status,
                Factions = factions
                    .Select(f => new SelectListItem
                    {
                        Text = f.Name,
                        Value = f.Id.ToString()
                    })
                    .ToList()

                

            };

            return View(model);
        }

      
        [HttpPost]
        public async Task<IActionResult> Edit(EditMissionPostRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);

            await missionService.UpdateAsync(request);
            return RedirectToAction("List");
        }


     
        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            await missionService.DeleteAsync(id);
            return RedirectToAction("List");
        }

    }
}
