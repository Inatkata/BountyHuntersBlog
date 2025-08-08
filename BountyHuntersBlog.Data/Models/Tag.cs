using System.Collections.Generic;

namespace BountyHuntersBlog.Data.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public ICollection<MissionTag> MissionTags { get; set; } = new List<MissionTag>();
    }
}