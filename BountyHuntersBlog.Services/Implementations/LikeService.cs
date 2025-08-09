using BountyHuntersBlog.Data.Models;
using BountyHuntersBlog.Repositories.Interfaces;
using BountyHuntersBlog.Services.DTOs;
using BountyHuntersBlog.Services.Interfaces;

namespace BountyHuntersBlog.Services
{
    public class LikeService : ILikeService
    {
        private readonly ILikeRepository _repo;

        public LikeService(ILikeRepository repo) => _repo = repo;

        public async Task<LikeResultDto> ToggleMissionLikeAsync(int missionId, string userId)
        {
            var existing = await _repo.FindMissionLikeAsync(missionId, userId);
            bool nowLiked;
            if (existing is null)
            {
                await _repo.AddAsync(new Like { MissionId = missionId, UserId = userId });
                nowLiked = true;
            }
            else
            {
                await _repo.RemoveAsync(existing);
                nowLiked = false;
            }
            await _repo.SaveChangesAsync();

            var count = await _repo.CountForMissionAsync(missionId);
            return new LikeResultDto { IsLiked = nowLiked, TotalCount = count };
        }

        public async Task<LikeResultDto> ToggleCommentLikeAsync(int commentId, string userId)
        {
            var existing = await _repo.FindCommentLikeAsync(commentId, userId);
            bool nowLiked;
            if (existing is null)
            {
                await _repo.AddAsync(new Like { CommentId = commentId, UserId = userId });
                nowLiked = true;
            }
            else
            {
                await _repo.RemoveAsync(existing);
                nowLiked = false;
            }
            await _repo.SaveChangesAsync();

            var count = await _repo.CountForCommentAsync(commentId);
            return new LikeResultDto { IsLiked = nowLiked, TotalCount = count };
        }

        public async Task<bool> IsMissionLikedByUserAsync(int missionId, string userId) =>
            (await _repo.FindMissionLikeAsync(missionId, userId)) != null;

        public async Task<bool> IsCommentLikedByUserAsync(int commentId, string userId) =>
            (await _repo.FindCommentLikeAsync(commentId, userId)) != null;

        public Task<int> CountMissionLikesAsync(int missionId) => _repo.CountForMissionAsync(missionId);
        public Task<int> CountCommentLikesAsync(int commentId) => _repo.CountForCommentAsync(commentId);
    }
}
