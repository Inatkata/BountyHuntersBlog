using BountyHuntersBlog.Models.Domain;
using BountyHuntersBlog.Models.ViewModels;
using BountyHuntersBlog.Repositories;
using BountyHuntersBlog.Web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BountyHuntersBlog.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminFactionsController : Controller
    {
        private readonly IFactionRepository factionRepository;

        public AdminFactionsController(IFactionRepository factionRepository)
        {
            this.factionRepository = factionRepository;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Add")]
        public async Task<IActionResult> Add(AddFactionRequest addFactionRequest)
        {
            ValidateAddFactionRequest(addFactionRequest);

            if (!ModelState.IsValid)
            {
                return View();
            }

            var faction = new Faction
            {
                Name = addFactionRequest.Name,
                DisplayName = addFactionRequest.DisplayName
            };

            await factionRepository.AddAsync(faction);

            return RedirectToAction("List");
        }

        [HttpGet]
        [ActionName("List")]
        public async Task<IActionResult> List(
            string? searchQuery,
            string? sortBy,
            string? sortDirection,
            int pageSize = 3,
            int pageNumber = 1)
        {
            var totalRecords = await factionRepository.CountAsync();
            var totalPages = Math.Ceiling((decimal)totalRecords / pageSize);

            if (pageNumber > totalPages) pageNumber--;
            if (pageNumber < 1) pageNumber++;

            ViewBag.TotalPages = totalPages;
            ViewBag.SearchQuery = searchQuery;
            ViewBag.SortBy = sortBy;
            ViewBag.SortDirection = sortDirection;
            ViewBag.PageSize = pageSize;
            ViewBag.PageNumber = pageNumber;

            var factions = await factionRepository.GetAllAsync(searchQuery, sortBy, sortDirection, pageNumber, pageSize);

            return View(factions);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var faction = await factionRepository.GetAsync(id);

            if (faction != null)
            {
                var editRequest = new EditFactionRequest
                {
                    Id = faction.Id,
                    Name = faction.Name,
                    DisplayName = faction.DisplayName
                };

                return View(editRequest);
            }

            return View(null);
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

            var updatedFaction = await factionRepository.UpdateAsync(faction);

            // Optional: notification logic
            return RedirectToAction("Edit", new { id = request.Id });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(EditFactionRequest request)
        {
            var deletedFaction = await factionRepository.DeleteAsync(request.Id);

            if (deletedFaction != null)
            {
                return RedirectToAction("List");
            }

            return RedirectToAction("Edit", new { id = request.Id });
        }

        private void ValidateAddFactionRequest(AddFactionRequest request)
        {
            if (!string.IsNullOrWhiteSpace(request.Name) &&
                !string.IsNullOrWhiteSpace(request.DisplayName) &&
                request.Name == request.DisplayName)
            {
                ModelState.AddModelError("DisplayName", "Name cannot be the same as DisplayName");
            }
        }
    }
}
