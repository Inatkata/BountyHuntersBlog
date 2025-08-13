using BountyHuntersBlog.Data.Constants;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace BountyHuntersBlog.ViewModels.Admin.Tags
{
    public class AdminTagFormVM
    {
        public int Id { get; set; }

        [Required, MaxLength(ModelConstants.Category.NameMaxLength)]
        public string Name { get; set; } = null!;

        public string Title { get; set; } = null!;

        public bool IsDeleted { get; set; }

        // Нови полета за селекция на тагове
        public List<SelectListItem> Tags { get; set; } = new();

        public List<int> TagIds { get; set; } = new();
    }
}