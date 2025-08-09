using AutoMapper;
using BountyHuntersBlog.Data.Models;
using BountyHuntersBlog.Services.DTOs;
using BountyHuntersBlog.Services.Interfaces;
using BountyHuntersBlog.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BountyHuntersBlog.Web.Controllers
{
    public class MissionsController : Controller
    {
        private readonly IMissionService _service;
        private readonly IMapper _mapper;
        private readonly ICategoryService _categories;
        private readonly ITagService _tags;
        public MissionsController(IMissionService service, IMapper mapper, ICategoryService categories, ITagService tags)
        {
            _service = service;
            _mapper = mapper;
            _categories = categories;
            _tags = tags;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            const int pageSize = 10;
            var dtos = await _service.GetAllAsync(page, pageSize);
            var vms = _mapper.Map<IEnumerable<MissionViewModel>>(dtos);
            return View(vms);
        }

        public async Task<IActionResult> Details(int id)
        {
            var dto = await _service.GetByIdAsync(id);
            if (dto == null) return NotFound();
            return View(_mapper.Map<MissionViewModel>(dto));
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(MissionViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);
            var dto = _mapper.Map<MissionDto>(vm);
            await _service.CreateAsync(dto);
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var dto = await _service.GetByIdAsync(id);
            if (dto == null) return NotFound();

            var vm = _mapper.Map<MissionViewModel>(dto);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MissionViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var dto = _mapper.Map<MissionDto>(vm);
            await _service.UpdateAsync(vm.Id, dto);
            return RedirectToAction(nameof(Details), new { id = vm.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var dto = await _service.GetByIdAsync(id);
            if (dto == null) return NotFound();

            var vm = _mapper.Map<MissionViewModel>(dto);
            return View(vm);
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
    
