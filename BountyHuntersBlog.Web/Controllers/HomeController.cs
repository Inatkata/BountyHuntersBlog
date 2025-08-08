using BountyHuntersBlog.Models.ViewModels;
using BountyHuntersBlog.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BountyHuntersBlog.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMissionPostRepository missionPostRepository;
        private readonly IFactionRepository factionRepository;

        public HomeController(
            IMissionPostRepository missionPostRepository,
            IFactionRepository factionRepository)
        {
            this.missionPostRepository = missionPostRepository;
            this.factionRepository = factionRepository;
        }

        public async Task<IActionResult> Index(string? searchTerm = null, Guid? factionId = null)
        {
            var allFactions = await factionRepository.GetAllAsync();
            var allMissions = await missionPostRepository.GetAllAsync();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                allMissions = allMissions
                    .Where(x => x.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
                                || (x.ShortDescription?.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ?? false));
            }

            if (factionId.HasValue)
            {
                allMissions = allMissions
                    .Where(x => x.Factions.Any(f => f.Id == factionId.Value));
            }

            var model = new HomeViewModel
            {
                MissionPosts = allMissions,
                Factions = allFactions,
                SearchTerm = searchTerm,
                FactionId = factionId
            };

            return View(model);
        }
    }
}