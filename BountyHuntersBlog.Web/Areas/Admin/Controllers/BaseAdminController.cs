using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BountyHuntersBlog.Web.Areas.Admin.Controllers
{
    [Area("Admin")]              
    [Authorize(Roles = "Admin")]        
    public abstract class BaseAdminController : Controller
    {
        protected bool IsUserAuthenticated =>
            User?.Identity?.IsAuthenticated ?? false;

        protected string? GetUserId() =>
            IsUserAuthenticated ? User.FindFirstValue(ClaimTypes.NameIdentifier) : null;
    }
}