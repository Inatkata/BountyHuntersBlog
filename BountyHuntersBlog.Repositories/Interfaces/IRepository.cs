using System.Linq.Expressions;

namespace BountyHuntersBlog.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        // Tracked queries (for updates)
        IQueryable<T> All();

        // Read-only queries (no tracking)
        IQueryable<T> AllReadonly();

        // Async read methods
        Task<List<T>> AllAsync();
        Task<List<T>> AllReadonlyAsync();

        // Get by Id
        Task<T?> GetByIdAsync(object id);

        // Existence check
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);

        // Add
        Task AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);

        // Update / Delete
        void Update(T entity);
        void Delete(T entity);

        // Save
        Task<int> SaveChangesAsync(CancellationToken ct = default);
    }
}