using BountyHuntersBlog.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using BountyHuntersBlog.Repositories.Interfaces;

namespace BountyHuntersBlog.Repositories.Base
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly BountyHuntersDbContext _ctx;
        protected readonly DbSet<T> _dbSet;

        public Repository(BountyHuntersDbContext ctx)
        {
            _ctx = ctx;
            _dbSet = _ctx.Set<T>();
        }

        public Task<List<T>> AllAsync() => _dbSet.ToListAsync();
        public IQueryable<T> AllAsQueryable() => _dbSet.AsQueryable();

        public Task<T?> GetByIdAsync(object id) => _dbSet.FindAsync(id).AsTask();

        public Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
            => _dbSet.AnyAsync(predicate);

        public Task AddAsync(T entity) => _dbSet.AddAsync(entity).AsTask();
        public Task AddRangeAsync(IEnumerable<T> entities) => _dbSet.AddRangeAsync(entities);

        public void Update(T entity) => _dbSet.Update(entity);
        public void Delete(T entity) => _dbSet.Remove(entity);

        public Task<int> SaveChangesAsync(CancellationToken ct = default)
            => _ctx.SaveChangesAsync(ct);
    }
}