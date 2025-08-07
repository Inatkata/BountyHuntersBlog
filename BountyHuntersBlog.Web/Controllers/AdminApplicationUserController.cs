using BountyHuntersBlog.Models.Domain;
using BountyHuntersBlog.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BountyHuntersBlog.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminApplicationUsersController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;

        public AdminApplicationUsersController(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var applicationUsers = await userManager.Users.ToListAsync();

            var viewModel = new AdminApplicationUserListViewModel
            {
                ApplicationUsers = applicationUsers,
                CreateApplicationUser = new CreateApplicationUserViewModel()
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> List(AdminApplicationUserListViewModel request)
        {
            var model = request.CreateApplicationUser;

            var user = new ApplicationUser
            {
                UserName = model.Username,
                Email = model.Email,
                DisplayName = model.Username
            };

            var result = await userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                var role = model.IsAdmin ? "Admin" : "User";
                await userManager.AddToRoleAsync(user, role);
                return RedirectToAction("List");
            }

            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var user = await userManager.FindByIdAsync(id.ToString());

            if (user == null) return NotFound();

            var roles = await userManager.GetRolesAsync(user);

            var viewModel = new CreateApplicationUserViewModel
            {
                Id = Guid.Parse(user.Id),
                Username = user.UserName,
                Email = user.Email,
                IsAdmin = roles.Contains("Admin")
            };

            return View("Edit", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CreateApplicationUserViewModel model)
        {
            var user = await userManager.FindByIdAsync(model.Id.ToString());

            if (user == null) return NotFound();

            user.UserName = model.Username;
            user.Email = model.Email;
            user.DisplayName = model.Username;

            var result = await userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Failed to update user.");
                return View("Edit", model);
            }

            var roles = await userManager.GetRolesAsync(user);

            if (model.IsAdmin && !roles.Contains("Admin"))
            {
                await userManager.AddToRoleAsync(user, "Admin");
                await userManager.RemoveFromRoleAsync(user, "User");
            }
            else if (!model.IsAdmin && !roles.Contains("User"))
            {
                await userManager.AddToRoleAsync(user, "User");
                await userManager.RemoveFromRoleAsync(user, "Admin");
            }

            return RedirectToAction("List");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var user = await userManager.FindByIdAsync(id.ToString());

            if (user != null)
            {
                var result = await userManager.DeleteAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("List");
                }
            }

            return View("List");
        }
    }
}
