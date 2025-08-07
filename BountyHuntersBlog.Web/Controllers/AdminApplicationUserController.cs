using BountyHuntersBlog.Models.ViewModels;
using BountyHuntersBlog.Repositories;
using BountyHuntersBlog.Models.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BountyHuntersBlog.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminApplicationUsersController : Controller
    {
        private readonly IApplicationUserRepository ApplicationUserRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public AdminApplicationUsersController(IApplicationUserRepository ApplicationUserRepository,
            UserManager<ApplicationUser> userManager)
        {
            this.ApplicationUserRepository = ApplicationUserRepository;
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var ApplicationUsers = await userManager.Users.ToListAsync();

            var viewModel = new AdminApplicationUserListViewModel
            {
                ApplicationUsers = ApplicationUsers,
                CreateApplicationUser = new CreateApplicationUserViewModel()
            };

            return View(viewModel);
        }


        [HttpPost]
        public async Task<IActionResult> List(AdminApplicationUserListViewModel request)
        {
            var ApplicationUserVm = request.CreateApplicationUser;

            var ApplicationUser = new ApplicationUser
            {
                UserName = ApplicationUserVm.Username,
                Email = ApplicationUserVm.Email,
                DisplayName = ApplicationUserVm.Username
            };

            var result = await userManager.CreateAsync(ApplicationUser, ApplicationUserVm.Password);

            if (result.Succeeded)
            {
                var role = ApplicationUserVm.IsAdmin ? "Admin" : "User";
                await userManager.AddToRoleAsync(ApplicationUser, role);
                return RedirectToAction("List");
            }

            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var ApplicationUser = await userManager.FindByIdAsync(id.ToString());

            if (ApplicationUser == null)
                return NotFound();

            var roles = await userManager.GetRolesAsync(ApplicationUser);

            var viewModel = new CreateApplicationUserViewModel
            {
                Id = Guid.Parse(ApplicationUser.Id),
                Username = ApplicationUser.UserName,
                Email = ApplicationUser.Email,
                IsAdmin = roles.Contains("Admin")
            };

            return View("Edit", viewModel); 
        }
        [HttpPost]
        public async Task<IActionResult> Edit(CreateApplicationUserViewModel model)
        {
            var ApplicationUser = await userManager.FindByIdAsync(model.Id.ToString());

            if (ApplicationUser == null)
                return NotFound();

            ApplicationUser.UserName = model.Username;
            ApplicationUser.Email = model.Email;
            ApplicationUser.DisplayName = model.Username;

            var result = await userManager.UpdateAsync(ApplicationUser);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Failed to update ApplicationUser.");
                return View("Edit", model);
            }

            var roles = await userManager.GetRolesAsync(ApplicationUser);

            if (model.IsAdmin && !roles.Contains("Admin"))
            {
                await userManager.AddToRoleAsync(ApplicationUser, "Admin");
                await userManager.RemoveFromRoleAsync(ApplicationUser, "User");
            }
            else if (!model.IsAdmin && !roles.Contains("User"))
            {
                await userManager.AddToRoleAsync(ApplicationUser, "User");
                await userManager.RemoveFromRoleAsync(ApplicationUser, "Admin");
            }

            return RedirectToAction("List");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var ApplicationUser = await userManager.FindByIdAsync(id.ToString());

            if (ApplicationUser is not null)
            {
                var result = await userManager.DeleteAsync(ApplicationUser);

                if (result is not null && result.Succeeded)
                {
                    return RedirectToAction("List", "AdminApplicationUsers");
                }
            }

            return View("List");
        }
    }
}
