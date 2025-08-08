using BountyHuntersBlog.Models.Domain;
using BountyHuntersBlog.Models.Requests;
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
        private readonly UserManager<Hunter> userManager;

        public AdminMissionPostsController(
            IMissionPostRepository missionPostRepository,
            IFactionRepository factionRepository,
            UserManager<Hunter> userManager)
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
                FactionList = factions.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddMissionPostRequest model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var selectedFactions = await factionRepository.GetSelectedFactionsAsync(model.SelectedFactions);

            var currentUser = await userManager.GetUserAsync(User);

            var mission = new MissionPost
            {
                Title = model.Title,
                ShortDescription = model.ShortDescription,
                Content = model.Content,
                FeaturedImageUrl = model.FeaturedImageUrl,
                UrlHandle = model.UrlHandle,
                MissionDate = model.MissionDate,
                Visible = model.Visible,
                Factions = selectedFactions.ToList(),
                AuthorId = currentUser.Id
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
            var mission = await missionPostRepository.GetAsync(id);
            var allFactions = await factionRepository.GetAllAsync();

            if (mission == null)
                return RedirectToAction("List");

            var model = new EditMissionPostRequest
            {
                Id = mission.Id,
                AuthorId = mission.AuthorId,
                Title = mission.Title,
                ShortDescription = mission.ShortDescription,
                Content = mission.Content,
                FeaturedImageUrl = mission.FeaturedImageUrl,
                UrlHandle = mission.UrlHandle,
                MissionDate = mission.MissionDate,
                Visible = mission.Visible,
                SelectedFactions = mission.Factions.Select(f => f.Id).ToList(),
                FactionList = allFactions.Select(f => new SelectListItem
                {
                    Text = f.Name,
                    Value = f.Id.ToString()
                })
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditMissionPostRequest model)
        {
            if (!ModelState.IsValid)
            {
                model.FactionList = (await factionRepository.GetAllAsync()).Select(f => new SelectListItem
                {
                    Text = f.Name,
                    Value = f.Id.ToString()
                });

                return View(model);
            }

            var selectedFactions = await factionRepository.GetSelectedFactionsAsync(model.SelectedFactions);

            var updated = new MissionPost
            {
                Id = model.Id,
                AuthorId = model.AuthorId,
                Title = model.Title,
                ShortDescription = model.ShortDescription,
                Content = model.Content,
                FeaturedImageUrl = model.FeaturedImageUrl,
                UrlHandle = model.UrlHandle,
                MissionDate = model.MissionDate,
                Visible = model.Visible,
                Factions = selectedFactions.ToList()
            };

            var result = await missionPostRepository.UpdateAsync(updated);

            if (result == null)
                return RedirectToAction("Edit", new { id = model.Id });

            return RedirectToAction("List");
        }
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var mission = await missionPostRepository.GetAsync(id);

            if (mission == null)
                return RedirectToAction("List");

            return View(mission);
        }
        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var deleted = await missionPostRepository.DeleteAsync(id);

            if (deleted == null)
                return RedirectToAction("List");

            return RedirectToAction("List");
        }

    }
}
