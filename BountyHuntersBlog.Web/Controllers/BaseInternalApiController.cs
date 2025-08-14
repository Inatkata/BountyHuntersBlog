using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BountyHuntersBlog.Web.Controllers
{
    [ApiController]
    [Authorize]
    [Route("/api/internal/[controller]")]
    public abstract class BaseInternalApiController : ControllerBase
    {
        protected bool IsUserAuthenticated =>
            User?.Identity?.IsAuthenticated ?? false;

        protected string? GetUserId() =>
            IsUserAuthenticated ? User.FindFirstValue(ClaimTypes.NameIdentifier) : null;
    }
}