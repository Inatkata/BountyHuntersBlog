using BountyHuntersBlog.Services.DTOs;

public interface ILikeService
{

    Task<IEnumerable<LikeDto>> GetLikesByMissionIdAsync(int missionId);
    Task<IEnumerable<LikeDto>> GetLikesByUserIdAsync(string userId);
    Task<int> CountLikesByMissionIdAsync(int missionId);
    Task<int> CountLikesByUserIdAsync(string userId);
    Task<bool> IsLikedByUserAsync(int missionId, string userId);
}