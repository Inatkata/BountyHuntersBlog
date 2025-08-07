using BountyHuntersBlog.Models.Domain;
using BountyHuntersBlog.Models.ViewModels;
using BountyHuntersBlog.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BountyHuntersBlog.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminMissionPostsController : Controller
    {
        private readonly IMissionPostRepository missionPostRepository;
        private readonly IFactionRepository factionRepository;

        public AdminMissionPostsController(IMissionPostRepository missionPostRepository, IFactionRepository factionRepository)
        {
            this.missionPostRepository = missionPostRepository;
            this.factionRepository = factionRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var factions = await factionRepository.GetAllAsync();
            var model = new AddMissionPostRequest
            {
                MissionDate = DateTime.Now,
                SelectedFactions = new List<Guid>(),
            };

            ViewBag.Factions = factions.Select(f => new SelectListItem
            {
                Text = f.Name,
                Value = f.Id.ToString()
            });

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddMissionPostRequest request)
        {
            var factions = await factionRepository.GetAllAsync();

            if (!ModelState.IsValid)
            {
                ViewBag.Factions = factions.Select(f => new SelectListItem
                {
                    Text = f.Name,
                    Value = f.Id.ToString()
                });
                return View(request);
            }

            var selectedFactions = factions.Where(f => request.SelectedFactions.Contains(f.Id)).ToList();

            var post = new MissionPost
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Content = request.Content,
                UrlHandle = request.UrlHandle,
                FeaturedImageUrl = request.FeaturedImageUrl,
                MissionDate = request.MissionDate,
                Visible = request.Visible,
                AuthorId = request.AuthorId,
                Factions = selectedFactions
            };

            await missionPostRepository.AddAsync(post, request.SelectedFactions.ToList());

            return RedirectToAction("List");
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var posts = await missionPostRepository.GetAllAsync();
            return View(posts);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var post = await missionPostRepository.GetAsync(id);
            if (post == null) return NotFound();

            var factions = await factionRepository.GetAllAsync();

            var model = new AddMissionPostRequest
            {
                Title = post.Title,
                Content = post.Content,
                UrlHandle = post.UrlHandle,
                FeaturedImageUrl = post.FeaturedImageUrl,
                MissionDate = post.MissionDate,
                Visible = post.Visible,
                AuthorId = post.AuthorId,
                SelectedFactions = post.Factions.Select(f => f.Id).ToList()
            };

            ViewBag.Factions = factions.Select(f => new SelectListItem
            {
                Text = f.Name,
                Value = f.Id.ToString(),
                Selected = model.SelectedFactions.Contains(f.Id)
            });

            ViewBag.PostId = post.Id;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, AddMissionPostRequest request)
        {
            var post = await missionPostRepository.GetAsync(id);
            if (post == null) return NotFound();

            var factions = await factionRepository.GetAllAsync();
            var selectedFactions = factions.Where(f => request.SelectedFactions.Contains(f.Id)).ToList();

            post.Title = request.Title;
            post.Content = request.Content;
            post.UrlHandle = request.UrlHandle;
            post.FeaturedImageUrl = request.FeaturedImageUrl;
            post.MissionDate = request.MissionDate;
            post.Visible = request.Visible;
            post.Factions = selectedFactions;

            await missionPostRepository.UpdateAsync(post, request.SelectedFactions.ToList());

            return RedirectToAction("List");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            await missionPostRepository.DeleteAsync(id);
            return RedirectToAction("List");
        }
    }
}
