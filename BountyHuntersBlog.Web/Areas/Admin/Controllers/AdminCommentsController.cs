using AutoMapper;
using BountyHuntersBlog.Services.DTOs;
using BountyHuntersBlog.Services.Interfaces;
using BountyHuntersBlog.ViewModels.Admin.Comments; // <-- admin VMs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BountyHuntersBlog.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminCommentsController : Controller
    {
        private readonly ICommentService _comments;
        private readonly IMapper _mapper;

        public AdminCommentsController(ICommentService comments, IMapper mapper)
        {
            _comments = comments;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var list = (await _comments.AllAsync())
                .Select(_mapper.Map<AdminCommentListItemVM>)
                .ToList();
            return View(list);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _comments.SoftDeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}