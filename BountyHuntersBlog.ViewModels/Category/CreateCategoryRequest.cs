using System.ComponentModel.DataAnnotations;
using BountyHuntersBlog.Data.Constants;

namespace BountyHuntersBlog.ViewModels.Categories
{
    public class CreateCategoryRequest
    {
        [Required]
        [MaxLength(ModelConstants.Category.NameMaxLength)]
        public string Name { get; set; } = null!;
    }
}