using BountyHuntersBlog.Models.Domain;
using BountyHuntersBlog.Models.ViewModels;
using BountyHuntersBlog.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BountyHuntersBlog.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminHuntersController : Controller
    {
        private readonly IHunterRepository hunterRepository;
        private readonly UserManager<Hunter> userManager;

        public AdminHuntersController(IHunterRepository hunterRepository,
            UserManager<Hunter> userManager)
        {
            this.hunterRepository = hunterRepository;
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var hunters = await userManager.Users.ToListAsync();

            var viewModel = new AdminHunterListViewModel
            {
                Hunters = hunters,
                CreateHunter = new CreateHunterViewModel()
            };

            return View(viewModel);
        }


        [HttpPost]
        public async Task<IActionResult> List(AdminHunterListViewModel request)
        {
            var hunterVm = request.CreateHunter;

            var hunter = new Hunter
            {
                UserName = hunterVm.Hunter,
                Email = hunterVm.Email,
                DisplayName = hunterVm.Hunter
            };

            var result = await userManager.CreateAsync(hunter, hunterVm.Password);

            if (result.Succeeded)
            {
                var role = hunterVm.IsAdmin ? "Admin" : "Hunter";
                await userManager.AddToRoleAsync(hunter, role);
                return RedirectToAction("List");
            }

            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var hunter = await userManager.FindByIdAsync(id.ToString());

            if (hunter == null)
                return NotFound();

            var roles = await userManager.GetRolesAsync(hunter);

            var viewModel = new CreateHunterViewModel
            {
                Id = Guid.Parse(hunter.Id),
                Hunter = hunter.UserName,
                Email = hunter.Email,
                IsAdmin = roles.Contains("Admin")
            };

            return View("Edit", viewModel); // Използваме Add.cshtml и същия ViewModel
        }
        [HttpPost]
        public async Task<IActionResult> Edit(CreateHunterViewModel model)
        {
            var hunter = await userManager.FindByIdAsync(model.Id.ToString());

            if (hunter == null)
                return NotFound();

            hunter.UserName = model.Hunter;
            hunter.Email = model.Email;
            hunter.DisplayName = model.Hunter;

            var result = await userManager.UpdateAsync(hunter);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Failed to update hunter.");
                return View("Edit", model);
            }

            var roles = await userManager.GetRolesAsync(hunter);

            if (model.IsAdmin && !roles.Contains("Admin"))
            {
                await userManager.AddToRoleAsync(hunter, "Admin");
                await userManager.RemoveFromRoleAsync(hunter, "Hunter");
            }
            else if (!model.IsAdmin && !roles.Contains("Hunter"))
            {
                await userManager.AddToRoleAsync(hunter, "Hunter");
                await userManager.RemoveFromRoleAsync(hunter, "Admin");
            }

            return RedirectToAction("List");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var hunter = await userManager.FindByIdAsync(id.ToString());

            if (hunter is not null)
            {
                var result = await userManager.DeleteAsync(hunter);

                if (result is not null && result.Succeeded)
                {
                    return RedirectToAction("List", "AdminHunters");
                }
            }

            return View("List");
        }
    }
}
