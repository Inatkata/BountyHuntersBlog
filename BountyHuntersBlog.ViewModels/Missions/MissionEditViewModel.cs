// BountyHuntersBlog.ViewModels/Missions/MissionEditViewModel.cs

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BountyHuntersBlog.ViewModels.Missions
{
    public class MissionEditViewModel
    {
        public int Id { get; set; }
        [Required, StringLength(200)]
        public string Title { get; set; } = null!;
        [Required, StringLength(4000)]
        public string Description { get; set; } = null!;
        [Required]
        public int CategoryId { get; set; }
        public List<int> TagIds { get; set; } = new();
        public IEnumerable<SelectListItem> Categories { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> Tags { get; set; } = new List<SelectListItem>();
    }
}