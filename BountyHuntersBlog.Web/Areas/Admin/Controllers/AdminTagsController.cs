using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using BountyHuntersBlog.Services.DTOs;
using BountyHuntersBlog.Services.Interfaces;
using BountyHuntersBlog.ViewModels.Admin.Tags;

namespace BountyHuntersBlog.Web.Areas.Admin.Controllers
{
    public class AdminTagsController : BaseAdminController
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

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AdminTagFormVM vm)
        {
            if (!ModelState.IsValid) return View(vm);

            await _tags.CreateAsync(new TagDto { Name = vm.Name, IsDeleted = vm.IsDeleted });
            TempData["Success"] = "Tag created.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var dto = await _tags.GetByIdAsync(id);
            if (dto == null) return NotFound();

            var vm = _mapper.Map<AdminTagFormVM>(dto);
            return View(vm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AdminTagFormVM vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var ok = await _tags.UpdateAsync(new TagDto { Id = vm.Id, Name = vm.Name, IsDeleted = vm.IsDeleted });
            if (!ok) return NotFound();

            TempData["Success"] = "Tag updated.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _tags.SoftDeleteAsync(id);
            TempData["Success"] = "Tag deleted.";
            return RedirectToAction(nameof(Index));
        }
    }
}
