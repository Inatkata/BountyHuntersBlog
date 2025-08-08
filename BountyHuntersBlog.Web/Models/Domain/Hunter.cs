using System.ComponentModel.DataAnnotations;

namespace BountyHuntersBlog.Models.Domain
{
    public class Hunter
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string DisplayName { get; set; }

        public string? Bio { get; set; }

        public string? ExperienceLevel { get; set; }

        public DateTime JoinedOn { get; set; }


        public ApplicationUser? ApplicationUser { get; set; }
    }
}