using BountyHuntersBlog.Services.DTOs;

namespace BountyHuntersBlog.ViewModels.Missions
{
    public class MissionDetailsVM
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string? Description { get; set; }
        public string ImageUrl { get; set; } = "/img/placeholder.png";
        public string? CategoryName { get; set; }
        public IEnumerable<string> Tags { get; set; } = Enumerable.Empty<string>();
        public int Likes { get; set; }

        public List<CommentVM> Comments { get; set; } = new();

        public static MissionDetailsVM FromDto(MissionDetailsDto d) => new()
        {
            Id = d.Id,
            Title = d.Title,
            Description = d.Description,
            ImageUrl = string.IsNullOrWhiteSpace(d.ImageUrl) ? "/img/placeholder.png" : d.ImageUrl!,
            CategoryName = d.CategoryName,
            Tags = d.TagNames ?? Enumerable.Empty<string>(),
            Likes = d.LikesCount,
            Comments = (d.Comments ?? new()).Select(c => new CommentVM
            {
                Id = c.Id,
                AuthorName = c.UserDisplayName,
                Content = c.Content,
                CreatedOn = c.CreatedOn
            }).ToList()
        };
        private MissionDetailsVM WithComments(IEnumerable<MissionCommentDetailsDto>? comments)
        {
            if (comments != null)
            {
                this.Comments = comments.Select(c => new CommentVM
                {
                    Id = c.Id,
                    AuthorName = c.UserDisplayName,
                    Content = c.Content,
                    CreatedOn = c.CreatedOn
                }).ToList();
            }
            return this;
        }
    }

    public class CommentVM
    {
        public int Id { get; set; }
        public string AuthorName { get; set; } = "";
        public string Content { get; set; } = "";
        public DateTime CreatedOn { get; set; }
    }
}