using System.Collections.Generic;

namespace BountyHuntersBlog.Data.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public ICollection<Mission> Missions { get; set; } = new List<Mission>();
    }
}