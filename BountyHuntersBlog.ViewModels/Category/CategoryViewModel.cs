using BountyHuntersBlog.Services.DTOs;

namespace BountyHuntersBlog.ViewModels.Category
{
    public class CategoryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        // брой мисии в категорията
        public int MissionsCount => Missions?.Count ?? 0;

        // за Details view
        public List<MissionDto> Missions { get; set; } = new();
    }
}