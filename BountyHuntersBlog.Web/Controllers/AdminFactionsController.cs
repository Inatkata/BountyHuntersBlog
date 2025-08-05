using BountyHuntersBlog.Models.Domain;
using BountyHuntersBlog.Models.ViewModels;
using BountyHuntersBlog.Repositories;
using BountyHuntersBlog.Web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BountyHuntersBlog.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminFactionsController : Controller
    {
        private readonly IFactionRepository factionRepository;

        public AdminFactionsController(IFactionRepository factionRepository)
        {
            this.factionRepository = factionRepository;
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var factions = await factionRepository.GetAllAsync();
            return View(factions);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddFactionRequest request)
        {
            var faction = new Faction
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                DisplayName = request.DisplayName
            };

            await factionRepository.AddAsync(faction);
            return RedirectToAction("List");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var faction = await factionRepository.GetAsync(id);
            if (faction == null) return NotFound();

            var model = new EditFactionRequest
            {
                Id = faction.Id,
                Name = faction.Name,
                DisplayName = faction.DisplayName
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditFactionRequest request)
        {
            var faction = new Faction
            {
                Id = request.Id,
                Name = request.Name,
                DisplayName = request.DisplayName
            };

            await factionRepository.UpdateAsync(faction);
            return RedirectToAction("List");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(EditFactionRequest request)
        {
            await factionRepository.DeleteAsync(request.Id);
            return RedirectToAction("List");
        }
    }
}
