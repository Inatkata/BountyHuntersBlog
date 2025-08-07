namespace BountyHuntersBlog.Models.Requests
{
    public class EditFactionRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
    }
}
