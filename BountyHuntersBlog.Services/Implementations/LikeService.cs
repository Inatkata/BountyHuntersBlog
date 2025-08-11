// Services/Implementations/LikeService.cs
using BountyHuntersBlog.Repositories.Interfaces;
using BountyHuntersBlog.Services.DTOs;
using BountyHuntersBlog.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BountyHuntersBlog.Services.Implementations
{
    public class LikeService : ILikeService
    {
        private readonly ILikeRepository _likes;

        public LikeService(ILikeRepository likes)
        {
            _likes = likes;
        }

        public async Task<LikeResultDto> ToggleMissionLikeAsync(int missionId, string userId)
        {
            var existing = await _likes.FindForMissionAsync(userId, missionId);
            bool liked;
            if (existing != null)
            {
                _likes.Delete(existing);
                liked = false;
            }
            else
            {
                await _likes.AddAsync(new Data.Models.Like { MissionId = missionId, UserId = userId });
                liked = true;
            }
            await _likes.SaveChangesAsync();

            var count = await _likes.AllAsQueryable().CountAsync(l => l.MissionId == missionId);
            return new LikeResultDto { TargetType = "mission", TargetId = missionId, IsLiked = liked, LikesCount = count };
        }

        public async Task<LikeResultDto> ToggleCommentLikeAsync(int commentId, string userId)
        {
            var existing = await _likes.FindForCommentAsync(userId, commentId);
            bool liked;
            if (existing != null)
            {
                _likes.Delete(existing);
                liked = false;
            }
            else
            {
                await _likes.AddAsync(new Data.Models.Like { CommentId = commentId, UserId = userId });
                liked = true;
            }
            await _likes.SaveChangesAsync();

            var count = await _likes.AllAsQueryable().CountAsync(l => l.CommentId == commentId);
            return new LikeResultDto { TargetType = "comment", TargetId = commentId, IsLiked = liked, LikesCount = count };
        }
    }
}
