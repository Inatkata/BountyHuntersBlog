using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using BountyHuntersBlog.Services.Interfaces;
using BountyHuntersBlog.ViewModels.Admin.Comments;

namespace BountyHuntersBlog.Web.Areas.Admin.Controllers
{
    public class AdminCommentsController : BaseAdminController
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
                .OrderByDescending(x => x.CreatedOn)
                .ToList();

            return View(list);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var dto = await _comments.GetByIdAsync(id);
            if (dto == null) return NotFound();

            var vm = _mapper.Map<AdminCommentListItemVM>(dto);
            return View(vm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, Services.DTOs.CommentDto ok)
        {
           
                await _comments.SoftDeleteAsync(id);
                TempData["Success"] = "Comment deleted.";
                return RedirectToAction(nameof(Index));
            

        }
    }
}