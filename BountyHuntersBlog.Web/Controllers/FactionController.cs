using BountyHuntersBlog.Models.ViewModels;
using BountyHuntersBlog.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BountyHuntersBlog.Controllers
{
    public class FactionController : Controller
    {
        private readonly IFactionRepository factionRepository;

        public FactionController(IFactionRepository factionRepository)
        {
            this.factionRepository = factionRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var factions = await factionRepository.GetAllAsync();
            return View(factions);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var faction = await factionRepository.GetByIdAsync(id);
            if (faction == null)
            {
                return NotFound();
            }
            return View(faction);
        }
    }
}