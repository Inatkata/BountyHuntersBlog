using System.ComponentModel.DataAnnotations;

namespace BountyHuntersBlog.Models.ViewModels
{
    public class AddHunterRequest
    {
        [Required]
        public string DisplayName { get; set; }

        public string? Bio { get; set; }

        public string? ExperienceLevel { get; set; }

        public DateTime JoinedOn { get; set; }
    }
}