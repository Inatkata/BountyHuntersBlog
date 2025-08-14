using BountyHuntersBlog.Services.DTOs;

namespace BountyHuntersBlog.ViewModels.Category
{
public class CategoryViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public List<MissionDto>? Missions { get; set; }
}
}
