using BountyHuntersBlog.Models.Domain;
using BountyHuntersBlog.Models.ViewModels;
using BountyHuntersBlog.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BountyHuntersBlog.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminFactionController : Controller
    {
        private readonly IFactionRepository factionRepository;

        public AdminFactionController(IFactionRepository factionRepository)
        {
            this.factionRepository = factionRepository;
        }

        [HttpGet]
        public IActionResult Add() => View();

        [HttpPost]
        public async Task<IActionResult> Add(AddFactionRequest request)
        {
            if (ModelState.IsValid)
            {
                var faction = new Faction
                {
                    Id = Guid.NewGuid(),
                    Name = request.Name,
                    Description = request.Description
                };

                await factionRepository.AddAsync(faction);
                return RedirectToAction("List");
            }

            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var factions = await factionRepository.GetAllAsync();
            return View(factions);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var faction = await factionRepository.GetAsync(id);
            if (faction == null) return RedirectToAction("List");

            var model = new AddFactionRequest
            {
                Name = faction.Name,
                Description = faction.Description
            };

            ViewBag.Id = faction.Id;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, AddFactionRequest request)
        {
            var updated = new Faction
            {
                Id = id,
                Name = request.Name,
                Description = request.Description
            };

            await factionRepository.UpdateAsync(updated);
            return RedirectToAction("List");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            await factionRepository.DeleteAsync(id);
            return RedirectToAction("List");
        }
    }
}
