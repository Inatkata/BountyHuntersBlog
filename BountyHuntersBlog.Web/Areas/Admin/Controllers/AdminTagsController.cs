using AutoMapper;
using BountyHuntersBlog.Services.DTOs;
using BountyHuntersBlog.Services.Interfaces;
using BountyHuntersBlog.ViewModels.Admin.Tags;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BountyHuntersBlog.ViewModels.Admin.Tags; 


namespace BountyHuntersBlog.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminTagsController : Controller
    {
        private readonly ITagService _tags;
        private readonly IMapper _mapper;

        public AdminTagsController(ITagService tags, IMapper mapper)
        {
            _tags = tags;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var list = (await _tags.AllAsync())
                .Select(_mapper.Map<AdminTagListItemVM>)
                .ToList();
            return View(list);
        }

        [HttpGet]
        public IActionResult Create() => View(new AdminTagFormVM());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AdminTagFormVM vm)
        {
            if (!ModelState.IsValid) return View(vm);

            await _tags.CreateAsync(new TagDto
            {
                Name = vm.Name,
                IsDeleted = vm.IsDeleted
            });

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var dto = await _tags.GetAsync(id);
            if (dto == null) return NotFound();

            var vm = _mapper.Map<AdminTagFormVM>(dto);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AdminTagFormVM vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var ok = await _tags.UpdateAsync(new TagDto
            {
                Id = vm.Id,
                Name = vm.Name,
                IsDeleted = vm.IsDeleted
            });

            if (!ok) return NotFound();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _tags.SoftDeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
