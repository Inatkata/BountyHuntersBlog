// Services/Interfaces/IMissionTagService.cs
namespace BountyHuntersBlog.Services.Interfaces
{
    public interface IMissionTagService
    {
        Task UpdateTagsAsync(int missionId, IEnumerable<int> tagIds);
    }
}