using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BountyHuntersBlog.Data.Models;
using BountyHuntersBlog.ViewModels.Auth;

public class AccountController : Controller
{
    private readonly SignInManager<ApplicationUser> _signIn;
    private readonly UserManager<ApplicationUser> _users;

    public AccountController(SignInManager<ApplicationUser> signIn, UserManager<ApplicationUser> users)
    {
        _signIn = signIn; _users = users;
    }

    [HttpGet]
    public IActionResult Login(string? returnUrl = null) => View(new LoginVM { ReturnUrl = returnUrl });

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginVM vm)
    {
        if (!ModelState.IsValid) return View(vm);
        var res = await _signIn.PasswordSignInAsync(vm.UserName, vm.Password, vm.RememberMe, lockoutOnFailure: true);
        if (res.Succeeded) return Redirect(vm.ReturnUrl ?? Url.Action("Index", "Home")!);
        ModelState.AddModelError("", "Invalid credentials.");
        return View(vm);
    }

    [HttpGet]
    public IActionResult Register() => View(new RegisterVM());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterVM vm)
    {
        if (!ModelState.IsValid) return View(vm);
        var user = new ApplicationUser { UserName = vm.UserName, Email = vm.Email };
        var res = await _users.CreateAsync(user, vm.Password);
        if (res.Succeeded)
        {
            await _users.AddToRoleAsync(user, "User");
            await _signIn.SignInAsync(user, isPersistent: false);
            return RedirectToAction("Index", "Home");
        }
        foreach (var e in res.Errors) ModelState.AddModelError("", e.Description);
        return View(vm);
    }

    [Authorize]
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signIn.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
}
