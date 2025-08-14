// BountyHuntersBlog.Web/Controllers/HomeController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BountyHuntersBlog.Web.Controllers
{
    public class HomeController : BaseController
    {
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Index() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int? statusCode)
        {

            return RedirectToAction("Index", "Error");

        }
    }
}