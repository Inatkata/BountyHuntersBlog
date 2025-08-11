using Microsoft.AspNetCore.Mvc;

namespace BountyHuntersBlog.Web.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/{code:int}")]
        public IActionResult Status(int code)
            => code == 404 ? View("NotFound") : View("ServerError");

        [Route("Error")]
        public IActionResult Index() => View("ServerError");
    }
}