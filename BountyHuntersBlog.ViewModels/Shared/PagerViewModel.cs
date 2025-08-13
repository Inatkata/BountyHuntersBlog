namespace BountyHuntersBlog.ViewModels.Shared
{
    public class PagerViewModel
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalCount { get; set; } = 0;

        public string Action { get; set; } = "Index";
        public string Controller { get; set; } = "";
        public string? Area { get; set; }

        // extra filters to keep in the query string
        public IDictionary<string, string?> RouteValues { get; set; } = new Dictionary<string, string?>();
    }
}