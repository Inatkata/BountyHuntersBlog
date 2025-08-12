// ViewModels/Admin/Categories/AdminCategoryListItemVM.cs
using System;

namespace BountyHuntersBlog.ViewModels.Admin.Categories
{
    public class AdminCategoryListItemVM
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public bool IsDeleted { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int MissionsCount { get; set; }
    }
}