using Microsoft.AspNetCore.Mvc;

namespace BountyHuntersBlog.Web.Areas.Admin.Controllers
{
    public class AdminHomeController : BaseAdminController
    {
        [HttpGet]
        public IActionResult Index() => View();
    }
}