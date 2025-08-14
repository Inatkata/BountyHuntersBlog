namespace BountyHuntersBlog.ViewModels.Shared
{
    public class PaginationViewModel
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        public string? Controller { get; set; }
        public string? Action { get; set; }
        public IDictionary<string, string?> RouteValues { get; set; }
            = new Dictionary<string, string?>();
    }
}