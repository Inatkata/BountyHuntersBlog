// Interfaces/ICommentRepository.cs
using BountyHuntersBlog.Data.Models;

namespace BountyHuntersBlog.Repositories.Interfaces
{
    public interface ICommentRepository : IRepository<Comment>
    {
        Task<Comment?> GetByIdWithIncludesAsync(int id);
    }
}