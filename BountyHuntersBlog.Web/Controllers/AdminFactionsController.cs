using BountyHuntersBlog.Models.Domain;
using BountyHuntersBlog.Models.Requests;
using BountyHuntersBlog.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BountyHuntersBlog.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminFactionsController : Controller
    {
        private readonly IFactionService factionService;

        public AdminFactionsController(IFactionService factionService)
        {
            this.factionService = factionService;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddFactionRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var faction = new Faction
            {
                Name = request.Name,
                DisplayName = request.DisplayName
            };

            await factionService.AddAsync(faction);

            return RedirectToAction("List");
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var factions = await factionService.GetAllAsync();
            return View(factions);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var faction = await factionService.GetByIdAsync(id);
            if (faction == null)
            {
                return RedirectToAction("List");
            }

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

            await factionService.UpdateAsync(faction);

            return RedirectToAction("Edit", new { id = faction.Id });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(EditFactionRequest request)
        {
            await factionService.DeleteAsync(request.Id);

            return RedirectToAction("List");
        }
    }
}
