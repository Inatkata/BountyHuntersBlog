using System.Linq.Expressions;

namespace BountyHuntersBlog.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> AllAsync();
        IQueryable<T> AllAsQueryable();
        Task<T?> GetByIdAsync(object id);
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);

        Task AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        void Update(T entity);
        void Delete(T entity);

        Task<int> SaveChangesAsync(CancellationToken ct = default);
    }
}