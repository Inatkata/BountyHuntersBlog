// ViewModels/Admin/Tags/AdminTagListItemVM.cs
namespace BountyHuntersBlog.ViewModels.Admin.Tags
{
    public class AdminTagListItemVM
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public bool IsDeleted { get; set; }
        public int MissionsCount { get; set; }
    }
}