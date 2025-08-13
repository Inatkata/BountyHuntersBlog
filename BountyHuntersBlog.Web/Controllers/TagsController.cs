using AutoMapper;
using BountyHuntersBlog.Services.Interfaces;
using BountyHuntersBlog.ViewModels.Missions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using BountyHuntersBlog.ViewModels.Tag;

namespace BountyHuntersBlog.Web.Controllers
{
    public class TagsController : Controller
    {
        private readonly ITagService _tags;
        private readonly IMissionService _missions;
        private readonly ICategoryService _categories;
        private readonly IMapper _mapper;

        public TagsController(
            ITagService tags,
            IMissionService missions,
            ICategoryService categories,
            IMapper mapper)
        {
            _tags = tags;
            _missions = missions;
            _categories = categories;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var allTags = await _tags.AllAsync();
            var vm = allTags.Select(_mapper.Map<TagViewModel>).ToList();
            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id, int page = 1, int pageSize = 10)
        {
            var tag = await _tags.GetAsync(id);
            if (tag == null) return NotFound();

            var (items, total) = await _missions.SearchPagedAsync(null, null, id, page, pageSize);

            var vm = new MissionIndexViewModel
            {
                Q = null,
                CategoryId = null,
                TagId = id,
                Page = page,
                PageSize = pageSize,
                TotalCount = total,
                Items = items.Select(_mapper.Map<MissionListItemViewModel>).ToList(),
                Categories = (await _categories.AllAsync())
                    .Select(c => new SelectListItem(c.Name, c.Id.ToString())),
                Tags = (await _tags.AllAsync())
                    .Select(t => new SelectListItem(t.Name, t.Id.ToString()))
            };

            ViewData["TagName"] = tag.Name;
            return View(vm);
        }
    }
}
