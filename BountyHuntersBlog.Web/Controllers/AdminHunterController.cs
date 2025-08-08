using BountyHuntersBlog.Models.Domain;
using BountyHuntersBlog.Models.ViewModels;
using BountyHuntersBlog.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BountyHuntersBlog.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminHunterController : Controller
    {
        private readonly IHunterRepository hunterRepository;

        public AdminHunterController(IHunterRepository hunterRepository)
        {
            this.hunterRepository = hunterRepository;
        }

        [HttpGet]
        public IActionResult Add() => View();

        [HttpPost]
        public async Task<IActionResult> Add(AddHunterRequest request)
        {
            if (ModelState.IsValid)
            {
                var hunter = new Hunter
                {
                    Id = Guid.NewGuid(),
                    DisplayName = request.DisplayName,
                    Bio = request.Bio,
                    ExperienceLevel = request.ExperienceLevel,
                    JoinedOn = request.JoinedOn
                };

                await hunterRepository.AddAsync(hunter);
                return RedirectToAction("List");
            }

            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var hunters = await hunterRepository.GetAllAsync();
            return View(hunters);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var hunter = await hunterRepository.GetAsync(id);
            if (hunter == null) return RedirectToAction("List");

            var model = new AddHunterRequest
            {
                DisplayName = hunter.DisplayName,
                Bio = hunter.Bio,
                ExperienceLevel = hunter.ExperienceLevel,
                JoinedOn = hunter.JoinedOn
            };

            ViewBag.HunterId = hunter.Id;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, AddHunterRequest request)
        {
            var updated = new Hunter
            {
                Id = id,
                DisplayName = request.DisplayName,
                Bio = request.Bio,
                ExperienceLevel = request.ExperienceLevel,
                JoinedOn = request.JoinedOn
            };

            await hunterRepository.UpdateAsync(updated);
            return RedirectToAction("List");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            await hunterRepository.DeleteAsync(id);
            return RedirectToAction("List");
        }
    }
}
