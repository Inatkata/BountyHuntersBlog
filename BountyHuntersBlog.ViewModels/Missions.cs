using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BountyHuntersBlog.ViewModels.Missions
{
    public class MissionListItemVm
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public bool IsCompleted { get; set; }
        public string CategoryName { get; set; } = "";
        public IEnumerable<string>? TagNames { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? ShortDescription { get; set; }
        public int CommentsCount { get; set; }
        public int LikesCount { get; set; }

    }

    public class MissionIndexViewModel
    {
        public string? Q { get; set; }
        public int? CategoryId { get; set; }
        public int? TagId { get; set; }
        public string? Search { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalCount { get; set; }

        public IEnumerable<MissionListItemVm> Items { get; set; } = new List<MissionListItemVm>();

        public IEnumerable<SelectListItem> Categories { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> Tags { get; set; } = new List<SelectListItem>();
    }

    public class MissionDetailsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public string CategoryName { get; set; } = "";
        public IEnumerable<string>? TagNames { get; set; }
        public int LikesCount { get; set; }
        public List<MissionCommentVm> Comments { get; set; } = new();
    }

    public class MissionCommentVm
    {
        public int Id { get; set; }
        public string UserDisplayName { get; set; } = "";
        public DateTime CreatedOn { get; set; }
        public string Content { get; set; } = "";
        public bool CanDelete { get; set; }
    }

   

    public class MissionEditViewModel
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Title { get; set; } = null!;

        [Required, StringLength(5000)]
        public string Description { get; set; } = null!;

        [Url, StringLength(2048)]
        public string? ImageUrl { get; set; }

        [Range(1, int.MaxValue)]
        public int CategoryId { get; set; }

        public List<int>? TagIds { get; set; }

        public bool IsCompleted { get; set; }
        public bool IsDeleted { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> Tags { get; set; } = new List<SelectListItem>();
    }
}
