using BountyHuntersBlog.Models.Domain;
using BountyHuntersBlog.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BountyHuntersBlog.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<Hunter> userManager;
        private readonly SignInManager<Hunter> signInManager;

        public AccountController(UserManager<Hunter> userManager,
                                 SignInManager<Hunter> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                var hunter = new Hunter
                {
                    UserName = registerViewModel.Username,
                    Email = registerViewModel.Email,
                    DisplayName = registerViewModel.Username
                };

                var result = await userManager.CreateAsync(hunter, registerViewModel.Password);

                if (result.Succeeded)
                {
                    var role = registerViewModel.IsAdmin ? "Admin" : "Hunter";
                    var roleResult = await userManager.AddToRoleAsync(hunter, role);

                    if (roleResult.Succeeded)
                    {
                        return RedirectToAction("Login");
                    }
                }
            }

            return View(registerViewModel);
        }


        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            var model = new LoginViewModel
            {
                ReturnUrl = returnUrl
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var result = await signInManager.PasswordSignInAsync(loginViewModel.Username,
                                                                  loginViewModel.Password,
                                                                  false, false);

            if (result.Succeeded)
            {
                if (!string.IsNullOrWhiteSpace(loginViewModel.ReturnUrl))
                {
                    return Redirect(loginViewModel.ReturnUrl);
                }

                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
