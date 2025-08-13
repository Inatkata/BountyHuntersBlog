// ViewModels/Admin/Tags/AdminTagFormVM.cs
using BountyHuntersBlog.Data.Constants;
using System.ComponentModel.DataAnnotations;

namespace BountyHuntersBlog.ViewModels.Admin.Tags
{
    public class AdminTagFormVM
    {
        public int Id { get; set; }

        [Required, MaxLength(ModelConstants.Category.NameMaxLength)]

        public string Name { get; set; } = null!;

        public bool IsDeleted { get; set; }
    }
}