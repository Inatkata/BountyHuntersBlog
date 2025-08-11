using AutoMapper;
using BountyHuntersBlog.Services.DTOs;
using BountyHuntersBlog.Services.Interfaces;
using BountyHuntersBlog.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BountyHuntersBlog.Web.Controllers
{
    public class TagsController : Controller
    {
        private readonly ITagService _service;
        private readonly IMapper _mapper;

        public TagsController(ITagService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }
        [AllowAnonymous]
        public async Task<IActionResult> Index(int page = 1)
        {
            const int pageSize = 10;
            var dtos = await _service.GetAllAsync(page, pageSize);
            var vms = _mapper.Map<IEnumerable<TagViewModel>>(dtos);
            return View(vms);
        }
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var dto = await _service.GetByIdAsync(id);
            if (dto == null) return NotFound();
            return View(_mapper.Map<TagViewModel>(dto));
        }
        [Authorize]
        [HttpGet]
        public IActionResult Create() => View(new TagViewModel());
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TagViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);
            await _service.CreateAsync(_mapper.Map<TagDto>(vm));
            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var dto = await _service.GetByIdAsync(id);
            if (dto == null) return NotFound();
            return View(_mapper.Map<TagViewModel>(dto));
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TagViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);
            await _service.UpdateAsync(vm.Id, _mapper.Map<TagDto>(vm));
            return RedirectToAction(nameof(Details), new { id = vm.Id });
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var dto = await _service.GetByIdAsync(id);
            if (dto == null) return NotFound();
            return View(_mapper.Map<TagViewModel>(dto));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _service.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
