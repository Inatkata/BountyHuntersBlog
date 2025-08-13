
using AutoMapper;
using BountyHuntersBlog.Data;                
using BountyHuntersBlog.Data.Models;          
using BountyHuntersBlog.ViewModels.Admin.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BountyHuntersBlog.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminUsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly BountyHuntersDbContext _db;   
        private readonly IMapper _mapper;

        public AdminUsersController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            BountyHuntersDbContext db,
            IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? search, int page = 1, int pageSize = 20)
        {
            var q = _userManager.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim();
                q = q.Where(u => (u.UserName ?? "").Contains(s)
                                 || (u.Email ?? "").Contains(s)
                                 || (u.DisplayName ?? "").Contains(s));
            }

            var total = await q.CountAsync();
            var items = await q.OrderBy(u => u.UserName)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new AdminUserListItemVM
                {
                    Id = u.Id,
                    UserName = u.UserName ?? "",
                    Email = u.Email ?? "",
                    DisplayName = u.DisplayName
                })
                .ToListAsync();

            ViewBag.TotalCount = total;
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;

            return View(items);
        }


        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var roles = await _userManager.GetRolesAsync(user);

            var vm = new AdminUserDetailsVM
            {
                Id = user.Id,
                UserName = user.UserName ?? string.Empty,
                DisplayName = user.DisplayName,
                Email = user.Email ?? string.Empty,
                Roles = roles,
                Missions = await _db.Missions.Where(m => m.UserId == user.Id).ToListAsync(),
                Comments = await _db.Comments.Where(c => c.UserId == user.Id).ToListAsync(),
                LikesCount = await _db.Likes.CountAsync(l => l.UserId == user.Id),
                IsLockedOut = user.LockoutEnd.HasValue && user.LockoutEnd.Value.UtcDateTime > DateTime.UtcNow
            };

            return View(vm);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Lock(string id, int days = 7)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            user.LockoutEnd = DateTimeOffset.UtcNow.AddDays(Math.Max(1, days));
            await _userManager.UpdateAsync(user);
            return RedirectToAction(nameof(Index));
        }

        // POST: /Admin/AdminUsers/Unlock/USER_ID
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Unlock(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            user.LockoutEnd = null;
            await _userManager.ResetAccessFailedCountAsync(user);
            await _userManager.UpdateAsync(user);
            return RedirectToAction(nameof(Index));
        }

        // POST: /Admin/AdminUsers/AddToRole
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToRole(string id, string role)
        {
            if (string.IsNullOrWhiteSpace(role)) return RedirectToAction(nameof(Index));

            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            role = role.Trim();
            if (!await _roleManager.RoleExistsAsync(role))
                await _roleManager.CreateAsync(new IdentityRole(role));

            var result = await _userManager.AddToRoleAsync(user, role);
            if (!result.Succeeded)
                ModelState.AddModelError("", string.Join("; ", result.Errors.Select(e => e.Description)));

            return RedirectToAction(nameof(Index));
        }

        // POST: /Admin/AdminUsers/RemoveFromRole
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveFromRole(string id, string role)
        {
            if (string.IsNullOrWhiteSpace(role)) return RedirectToAction(nameof(Index));

            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var result = await _userManager.RemoveFromRoleAsync(user, role.Trim());
            if (!result.Succeeded)
                ModelState.AddModelError("", string.Join("; ", result.Errors.Select(e => e.Description)));

            return RedirectToAction(nameof(Index));
        }
    }
}
