using BountyHuntersBlog.Models.ViewModels;
using BountyHuntersBlog.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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
       


        [HttpGet]
        public async Task<IActionResult> Index(string? searchTerm, Guid? factionId)
        {
            ;

            var allPosts = await missionPostRepository.GetAllAsync();
            var allFactions = await factionRepository.GetAllAsync();
            
            
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                allPosts = allPosts.Where(p =>
                    p.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    p.Content.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
            }

            if (factionId.HasValue)
            {
                allPosts = allPosts.Where(p => p.Factions.Any(f => f.Id == factionId.Value));
            }

            var model = new HomeViewModel
            {
                MissionPosts = allPosts,
                Factions = allFactions,
                SearchTerm = searchTerm,
                FactionFilterId = factionId
            };

            ViewBag.Factions = allFactions.Select(f => new SelectListItem
            {
                Value = f.Id.ToString(),
                Text = f.Name,
                Selected = (f.Id == model.FactionFilterId)
            }).ToList(); ;

            return View(model);
        }
        


    }
}