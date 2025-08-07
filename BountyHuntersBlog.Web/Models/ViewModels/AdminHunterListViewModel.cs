using BountyHuntersBlog.Models.Domain;

namespace BountyHuntersBlog.Models.ViewModels
{
    public class AdminHunterListViewModel
    {
        public List<Hunter> Hunters { get; set; } = new();

        public CreateHunterViewModel CreateHunter { get; set; } = new();
    }
}