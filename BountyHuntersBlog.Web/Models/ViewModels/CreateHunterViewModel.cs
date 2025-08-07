using System.ComponentModel.DataAnnotations;

namespace BountyHuntersBlog.Models.ViewModels
{
    public class CreateHunterViewModel
    {
        public Guid? Id { get; set; }
        [Required]
        public string Hunter { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool IsAdmin { get; set; }
    }
}