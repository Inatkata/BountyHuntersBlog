using Microsoft.AspNetCore.Mvc;
using BountyHuntersBlog.Services.Interfaces;
using BountyHuntersBlog.ViewModels.Missions;

namespace BountyHuntersBlog.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMissionService _missions;

        public HomeController(IMissionService missions) => _missions = missions;

        [HttpGet]
        public async Task<IActionResult> Index(string? q, int? categoryId, int? tagId, int page = 1, int pageSize = 12)
        {
            var (items, total) = await _missions.SearchPagedAsync(q, categoryId, tagId, page, pageSize);

            var vm = new MissionsPagedVM
            {
                Q = q,
                CategoryId = categoryId,
                TagId = tagId,
                Page = page,
                PageSize = pageSize,
                TotalCount = total,
                Items = items.Select(d => new MissionCardVM
                {
                    Id = d.Id,
                    Title = d.Title,
                    ShortDescription = d.Description ?? string.Empty,   
                    ImageUrl = string.IsNullOrWhiteSpace(d.ImageUrl) ? "/img/placeholder.png" : d.ImageUrl!,
                    Likes = 0,        
                    Comments = 0   
                }).ToList()
            };


            vm.Categories = await _missions.GetCategoriesSelectListAsync();
            vm.Tags = await _missions.GetTagsSelectListAsync();

            return View(vm);
        }
    }
}