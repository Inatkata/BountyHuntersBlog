using System.ComponentModel.DataAnnotations;
using BountyHuntersBlog.Data.Constants;

namespace BountyHuntersBlog.ViewModels.Tags
{
    public class CreateTagRequest
    {
        [Required]
        [MaxLength(ModelConstants.Tag.NameMaxLength)]
        public string Name { get; set; } = null!;
    }
}