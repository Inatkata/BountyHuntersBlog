// Services/Interfaces/ILikeService.cs
using BountyHuntersBlog.Services.DTOs;

namespace BountyHuntersBlog.Services.Interfaces
{
    public interface ILikeService
    {
        Task<LikeResultDto> ToggleMissionLikeAsync(int missionId, string userId);
        Task<LikeResultDto> ToggleCommentLikeAsync(int commentId, string userId);
    }
}