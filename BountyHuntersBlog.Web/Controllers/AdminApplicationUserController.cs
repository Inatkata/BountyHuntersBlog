using BountyHuntersBlog.Models.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BountyHuntersBlog.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminApplicationUserController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;

        public AdminApplicationUserController(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<IActionResult> List()
        {
            var users = await userManager.Users.ToListAsync();
            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ApplicationUser updatedUser)
        {
            var user = await userManager.FindByIdAsync(updatedUser.Id);

            if (user != null)
            {
                user.UserName = updatedUser.UserName;
                user.Email = updatedUser.Email;

                var result = await userManager.UpdateAsync(user);

                if (result.Succeeded)
                    return RedirectToAction("List");
            }

            return RedirectToAction("List");
        }
    }
}