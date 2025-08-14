using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BountyHuntersBlog.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator")]
    public class AdminHomeController : Controller
    {
        [HttpGet]
        public IActionResult Index() => View(); // simple dashboard links
    }
}