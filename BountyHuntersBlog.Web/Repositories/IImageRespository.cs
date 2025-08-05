using Microsoft.AspNetCore.Http;

namespace BountyHuntersBlog.Repositories
{
    public interface IImageRepository
    {
        Task<string> UploadAsync(IFormFile file);
    }
}