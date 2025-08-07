using System.ComponentModel.DataAnnotations;

namespace BountyHuntersBlog.Models.ViewModels
{
    public class AddFactionRequest
    {
        [Required]
        public string Name { get; set; }

        public string? Description { get; set; }
    }
}