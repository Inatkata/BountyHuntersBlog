// ViewModels/Missions/MissionListItemViewModel.cs
using System;
using System.Collections.Generic;

namespace BountyHuntersBlog.ViewModels.Missions
{
    public class MissionListItemViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string ShortDescription => Description?.Length > 160 ? Description[..160] + "…" : Description; // back-compat
        public DateTime CreatedOn { get; set; }

        public int? CategoryId { get; set; }             // back-compat
        public List<int> TagIds { get; set; } = new();   // back-compat
        public List<string> TagNames { get; set; } = new();

        public int CommentsCount { get; set; } = 0;      // back-compat
        public int LikesCount { get; set; } = 0;         // back-compat
        public bool IsCompleted { get; set; } = false; // back-compat
    }
}