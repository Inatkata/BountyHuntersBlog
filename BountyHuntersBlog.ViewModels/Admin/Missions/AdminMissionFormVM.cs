// ViewModels/Admin/Missions/AdminMissionFormVM.cs
using BountyHuntersBlog.Data.Constants;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BountyHuntersBlog.ViewModels.Admin.Missions
{
    public class AdminMissionFormVM
    {
        public int Id { get; set; }

        [Required, MaxLength(ModelConstants.Category.NameMaxLength)]

        public string Title { get; set; } = null!;

        [Required, StringLength(4000)]
        public string Description { get; set; } = null!;

        [Url]
        public string? ImageUrl { get; set; }

        [Required]
        public int CategoryId { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; } = new List<SelectListItem>();

        public IEnumerable<int> TagIds { get; set; } = new List<int>();
        public IEnumerable<SelectListItem> Tags { get; set; } = new List<SelectListItem>();

        public bool IsCompleted { get; set; }
        public bool IsDeleted { get; set; }
    }
}