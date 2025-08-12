namespace BountyHuntersBlog.ViewModels.Shared
{
    public class PaginationViewModel
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public string? BaseUrl { get; set; }
        public string? Query { get; set; }

        public int TotalPages => PageSize <= 0 ? 0 : (int)System.Math.Ceiling((double)TotalCount / PageSize);

        public string BuildUrl(int page)
        {
            var q = string.IsNullOrWhiteSpace(Query) ? "" : $"&q={System.Uri.EscapeDataString(Query)}";
            return $"{BaseUrl}?page={page}&pageSize={PageSize}{q}";
        }
    }
}