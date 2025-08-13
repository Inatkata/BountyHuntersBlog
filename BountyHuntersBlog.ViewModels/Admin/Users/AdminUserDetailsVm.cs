using BountyHuntersBlog.Data.Models;

namespace BountyHuntersBlog.ViewModels.Admin.Users
{
public class AdminUserDetailsVM
{
    public string Id { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string? DisplayName { get; set; }
    public string Email { get; set; } = null!;
    public IEnumerable<string> Roles { get; set; } = new List<string>();
    public IEnumerable<Mission> Missions { get; set; } = new List<Mission>();
    public IEnumerable<Comment> Comments { get; set; } = new List<Comment>();
    public int LikesCount { get; set; }
    public bool IsLockedOut { get; set; }
}
}
