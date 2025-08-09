namespace BountyHuntersBlog.Services.DTOs
{
    public class TagDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int MissionsCount { get; set; }
    }
}