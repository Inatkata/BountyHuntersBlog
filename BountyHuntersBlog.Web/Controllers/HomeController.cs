using BountyHuntersBlog.Models;
using BountyHuntersBlog.Models.ViewModels;
using BountyHuntersBlog.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BountyHuntersBlog.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMissionPostRepository missionPostRepository;
        private readonly IFactionRepository factionRepository;

        public HomeController(
            ILogger<HomeController> logger,
            IMissionPostRepository missionPostRepository,
            IFactionRepository factionRepository)
        {
            _logger = logger;
            this.missionPostRepository = missionPostRepository;
            this.factionRepository = factionRepository;
        }

        public async Task<IActionResult> Index()
        {
            var missions = await missionPostRepository.GetAllAsync();
            var factions = await factionRepository.GetAllAsync(null, null, null, 1, 100);

            var model = new HomeViewModel
            {
                MissionPosts = missions,
                Factions = factions
            };

            return View(model);
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