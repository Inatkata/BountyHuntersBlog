using BountyHuntersBlog.Models.Domain;
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
        public async Task<IActionResult> Add(Faction faction)
        {
            if (!ModelState.IsValid)
                return View(faction);

            faction.Id = Guid.NewGuid();
            await factionRepository.AddAsync(faction);
            return RedirectToAction("List");
        }
    }
}