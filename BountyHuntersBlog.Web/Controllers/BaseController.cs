using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BountyHuntersBlog.Web.Controllers
{
    [Authorize]
    [AutoValidateAntiforgeryToken]
    public abstract class BaseController : Controller
    {
        protected bool IsUserAuthenticated =>
            User?.Identity?.IsAuthenticated ?? false;

        protected string? GetUserId() =>
            IsUserAuthenticated ? User.FindFirstValue(ClaimTypes.NameIdentifier) : null;
    }
}