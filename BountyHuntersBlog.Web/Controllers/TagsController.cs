using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using BountyHuntersBlog.Services.Interfaces;
using BountyHuntersBlog.Services.DTOs;
using BountyHuntersBlog.ViewModels;

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

        public async Task<IActionResult> Index(int page = 1)
        {
            const int pageSize = 10;
            var dtos = await _service.GetAllAsync(page, pageSize);
            var vms = _mapper.Map<IEnumerable<TagViewModel>>(dtos);
            return View(vms);
        }

        public async Task<IActionResult> Details(int id)
        {
            var dto = await _service.GetByIdAsync(id);
            if (dto == null) return NotFound();
            return View(_mapper.Map<TagViewModel>(dto));
        }

        [HttpGet]
        public IActionResult Create() => View(new TagViewModel());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TagViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);
            await _service.CreateAsync(_mapper.Map<TagDto>(vm));
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var dto = await _service.GetByIdAsync(id);
            if (dto == null) return NotFound();
            return View(_mapper.Map<TagViewModel>(dto));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TagViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);
            await _service.UpdateAsync(vm.Id, _mapper.Map<TagDto>(vm));
            return RedirectToAction(nameof(Details), new { id = vm.Id });
        }

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
