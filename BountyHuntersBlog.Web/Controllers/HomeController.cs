using BountyHuntersBlog.Repositories;
using BountyHuntersBlog.Models;
using BountyHuntersBlog.Models.ViewModels;
using BountyHuntersBlog.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using BountyHuntersBlog.Web.Repositories;

namespace BountyHuntersBlog.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMissionPostRepository MissionPostRepository;
        private readonly IFactionRepository FactionRepository;

        public HomeController(ILogger<HomeController> logger,
            IMissionPostRepository MissionPostRepository,
            IFactionRepository FactionRepository
            )
        {
            _logger = logger;
            this.MissionPostRepository = MissionPostRepository;
            this.FactionRepository = FactionRepository;
        }

        public async Task<IActionResult> Index()
        {
            var missions = await MissionPostRepository.GetAllAsync();
            var factions = await FactionRepository.GetAllAsync();

            var model = new HomeViewModel
            {
                MissionPosts = missions
                    .Where(m => m.Visible)
                    .OrderByDescending(m => m.MissionDate)
                    .ToList(),
                Factions = factions.ToList()
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