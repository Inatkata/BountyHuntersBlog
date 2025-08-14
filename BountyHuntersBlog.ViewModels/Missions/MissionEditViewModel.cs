// BountyHuntersBlog.ViewModels/Missions/MissionEditViewModel.cs
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace BountyHuntersBlog.ViewModels.Missions
{
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