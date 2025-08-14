using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace BountyHuntersBlog.Web.Controllers
{
    public class TagsController : BaseController
    {

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Index() { /* ... */ }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Details(int id, int page = 1, int pageSize = 10) { /* ... */ }
    }
}