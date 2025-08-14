using BountyHuntersBlog.Data.Models; // ApplicationUser
using BountyHuntersBlog.ViewModels.Admin.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BountyHuntersBlog.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator")]
    public class AdminUsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _users;
        private readonly RoleManager<IdentityRole> _roles;

        public AdminUsersController(UserManager<ApplicationUser> users, RoleManager<IdentityRole> roles)
        {
            _users = users;
            _roles = roles;
        }

        // LIST + search
        [HttpGet]
        public async Task<IActionResult> Index(string? q)
        {
            var query = _users.Users.AsQueryable();
            if (!string.IsNullOrWhiteSpace(q))
            {
                q = q.Trim();
                query = query.Where(u => u.UserName!.Contains(q) || u.Email!.Contains(q));
            }

            var data = new List<AdminUserListItemVM>();
            await foreach (var u in query.AsAsyncEnumerable())
            {
                var roles = await _users.GetRolesAsync(u);
                data.Add(new AdminUserListItemVM
                {
                    Id = u.Id,
                    UserName = u.UserName ?? "",
                    Email = u.Email ?? "",
                    Roles = roles.ToList(),
                    IsLockedOut = u.LockoutEnd.HasValue && u.LockoutEnd.Value.UtcDateTime > DateTime.UtcNow
                });
            }

            ViewBag.Q = q;
            return View(data.OrderBy(x => x.UserName).ToList());
        }

        // EDIT ROLES
        [HttpGet]
        public async Task<IActionResult> EditRoles(string id)
        {
            var u = await _users.FindByIdAsync(id);
            if (u == null) return NotFound();

            var allRoles = _roles.Roles.Select(r => r.Name!).ToList();
            var userRoles = await _users.GetRolesAsync(u);

            var vm = new AdminUserRolesVM
            {
                UserId = u.Id,
                UserName = u.UserName ?? "",
                Email = u.Email ?? "",
                AvailableRoles = allRoles,
                SelectedRoles = userRoles.ToList()
            };
            return View(vm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRoles(AdminUserRolesVM vm)
        {
            var u = await _users.FindByIdAsync(vm.UserId);
            if (u == null) return NotFound();

            // ensure roles exist
            foreach (var r in vm.AvailableRoles ?? new List<string>())
                if (!await _roles.RoleExistsAsync(r)) await _roles.CreateAsync(new IdentityRole(r));

            var current = await _users.GetRolesAsync(u);
            var desired = vm.SelectedRoles?.Distinct().ToList() ?? new List<string>();

            var toAdd = desired.Except(current).ToList();
            var toRemove = current.Except(desired).ToList();

            if (toAdd.Any()) await _users.AddToRolesAsync(u, toAdd);
            if (toRemove.Any()) await _users.RemoveFromRolesAsync(u, toRemove);

            TempData["Msg"] = "Roles updated.";
            return RedirectToAction(nameof(Index));
        }

        // LOCK / UNLOCK
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Lock(string id, int days = 3650)
        {
            var u = await _users.FindByIdAsync(id);
            if (u == null) return NotFound();
            await _users.SetLockoutEnabledAsync(u, true);
            await _users.SetLockoutEndDateAsync(u, DateTimeOffset.UtcNow.AddDays(days));
            TempData["Msg"] = "User locked.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Unlock(string id)
        {
            var u = await _users.FindByIdAsync(id);
            if (u == null) return NotFound();
            await _users.SetLockoutEndDateAsync(u, DateTimeOffset.UtcNow);
            TempData["Msg"] = "User unlocked.";
            return RedirectToAction(nameof(Index));
        }

        // DELETE (HARD). Ако искаш soft-delete – смени логиката към флаг.
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            var u = await _users.FindByIdAsync(id);
            if (u == null) return NotFound();
            var me = await _users.GetUserAsync(User);
            if (me != null && me.Id == u.Id)
            {
                TempData["Msg"] = "You cannot delete yourself.";
                return RedirectToAction(nameof(Index));
            }
            var res = await _users.DeleteAsync(u);
            TempData["Msg"] = res.Succeeded ? "User deleted." : string.Join("; ", res.Errors.Select(e => e.Description));
            return RedirectToAction(nameof(Index));
        }
    }
}
