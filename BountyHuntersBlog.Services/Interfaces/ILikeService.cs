using BountyHuntersBlog.Services.DTOs;

namespace BountyHuntersBlog.Services.Interfaces
{
    public interface ILikeService 
    {
        Task<LikeResultDto> ToggleMissionLikeAsync(int missionId, string userId);
        Task<LikeResultDto> ToggleCommentLikeAsync(int commentId, string userId);

        Task<bool> IsMissionLikedByUserAsync(int missionId, string userId);
        Task<bool> IsCommentLikedByUserAsync(int commentId, string userId);

        Task<int> CountMissionLikesAsync(int missionId);
        Task<int> CountCommentLikesAsync(int commentId);
    }
}