using BountyHuntersBlog.Data.Constants;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BountyHuntersBlog.Data.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(ModelConstants.CategoryNameMaxLength)]
        public string Name { get; set; } = null!;

        public ICollection<Mission> Missions { get; set; } = new List<Mission>();
    }
}