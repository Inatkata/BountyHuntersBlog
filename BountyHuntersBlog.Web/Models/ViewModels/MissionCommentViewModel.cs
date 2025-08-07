using BountyHuntersBlog.Models.Domain;

namespace BountyHuntersBlog.Models.ViewModels
{

    public class MissionCommentViewModel
    {
        public string Description { get; set; }
        public DateTime DateAdded { get; set; }
        public string UserName { get; set; }
    }


}