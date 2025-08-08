namespace BountyHuntersBlog.Data.Models
{
    public class MissionTag
    {
        public int MissionId { get; set; }
        public Mission Mission { get; set; } = null!;

        public int TagId { get; set; }
        public Tag Tag { get; set; } = null!;
    }
}