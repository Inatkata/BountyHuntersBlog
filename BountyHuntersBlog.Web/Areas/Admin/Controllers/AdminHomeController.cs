using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BountyHuntersBlog.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminHomeController : Controller
    {
        [HttpGet]
        public IActionResult Index() => View();
    }
}