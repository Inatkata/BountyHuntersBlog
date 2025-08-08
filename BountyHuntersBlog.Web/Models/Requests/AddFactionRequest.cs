using System.ComponentModel.DataAnnotations;

namespace BountyHuntersBlog.Models.Requests
{
    public class AddFactionRequest
    {
        [Required]
        public string Name { get; set; }

        public string? Description { get; set; }
    }
}