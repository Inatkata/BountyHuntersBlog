// BountyHuntersBlog.ViewModels/Shared/PagedResult.cs
using System.Collections.Generic;

namespace BountyHuntersBlog.ViewModels.Shared
{
    public class PagedResult<T>
    {
        public List<T> Items { get; set; } = new();
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}