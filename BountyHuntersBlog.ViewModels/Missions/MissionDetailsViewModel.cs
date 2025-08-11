// ViewModels/Missions/MissionDetailsViewModel.cs
using System;
using System.Collections.Generic;

namespace BountyHuntersBlog.ViewModels.Missions
{
    public class MissionDetailsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime CreatedOn { get; set; }
        public DateTime CreatedOnUtc => CreatedOn;          // back-compat
        public bool IsCompleted { get; set; }

        public string UserId { get; set; } = null!;
        public string UserDisplayName { get; set; } = null!;
        public string AuthorDisplayName => UserDisplayName;  // back-compat

        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;

        public List<TagViewModel> Tags { get; set; } = new();
        public List<string> TagNames { get; set; } = new();  // back-compat

        public int LikesCount { get; set; }
        public int CommentsCount { get; set; }

        public bool CanLike { get; set; } = false;           // back-compat за Views

        public class CommentItem
        {
            public int Id { get; set; }
            public string Content { get; set; } = null!;
            public string Text { get => Content; set => Content = value; } // back-compat
            public DateTime CreatedOn { get; set; }
            public DateTime CreatedOnUtc => CreatedOn;                      // back-compat
            public string UserId { get; set; } = null!;
            public string UserDisplayName { get; set; } = null!;
            public string AuthorDisplayName => UserDisplayName;             // back-compat
            public int LikesCount { get; set; } = 0;                        // back-compat
            public bool CanLike { get; set; } = false;                      // back-compat
            public bool CanDelete { get; set; } = false;                    // back-compat
        }
        public List<CommentItem> Comments { get; set; } = new();
    }
}
