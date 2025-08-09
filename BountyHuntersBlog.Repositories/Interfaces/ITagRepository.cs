using BountyHuntersBlog.Data.Models;

namespace BountyHuntersBlog.Repositories.Interfaces
{
    public interface ITagRepository : IRepository<Tag>
    {
        Task<IReadOnlyList<Tag>> AllAsync();
        Task<Tag?> GetByIdAsync(int id);
        Task AddAsync(Tag entity);
        void Update(Tag entity);
        void Delete(Tag entity);
        Task<int> SaveChangesAsync();

        Task<bool> ExistsAsync(int id); 
    }
}