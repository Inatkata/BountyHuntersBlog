// Interfaces/IMissionRepository.cs
using BountyHuntersBlog.Data.Models;

namespace BountyHuntersBlog.Repositories.Interfaces
{
    public interface IMissionRepository : IRepository<Mission>
    {
        Task<Mission?> GetByIdWithIncludesAsync(int id);
        IQueryable<Mission> SearchQueryable(string? q, int? categoryId, int? tagId);
    }
}