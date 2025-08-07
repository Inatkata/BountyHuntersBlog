using System.ComponentModel.DataAnnotations;
using BountyHuntersBlog.Models.Domain;



    namespace BountyHuntersBlog.Models.Domain
    {
        public class MissionLike
        {
            public Guid Id { get; set; }

            public Guid MissionPostId { get; set; }
            public MissionPost MissionPost { get; set; } = null!;
        [Required]
            public string ApplicationUserId { get; set; } = null!;
            public ApplicationUser ApplicationUser { get; set; } = null!;
        }


}


