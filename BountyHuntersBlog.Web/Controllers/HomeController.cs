using BountyHuntersBlog.Models;
using BountyHuntersBlog.Models.ViewModels;
using BountyHuntersBlog.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

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
            var posts = await missionPostRepository.GetAllAsync();
            var factions = await factionRepository.GetAllAsync();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                posts = posts.Where(p => p.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
                    || p.Content.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (factionId.HasValue)
            {
                posts = posts.Where(p => p.FactionId == factionId).ToList();
            }

            var viewModel = new HomeViewModel
            {
                MissionPosts = posts,
                Factions = factions,
                SearchTerm = searchTerm,
                FactionId = factionId
            };

            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
