using BountyHuntersBlog.Data;
using BountyHuntersBlog.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BountyHuntersBlog.Repositories.Base
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly BountyHuntersDbContext Context;
        protected readonly DbSet<T> DbSet;
       
        public Repository(BountyHuntersDbContext context)
        {
            Context = context;
            DbSet = context.Set<T>();
        }

        public async Task<IEnumerable<T>> AllAsync()
            => await DbSet.ToListAsync();

        public async Task<T?> GetByIdAsync(int id)
            => await DbSet.FindAsync(id);

        public async Task AddAsync(T entity)
            => await DbSet.AddAsync(entity);

        public void Update(T entity)
            => DbSet.Update(entity);

        public void Delete(T entity) => DbSet.Remove(entity);
        public Task<int> SaveChangesAsync()
        => Context.SaveChangesAsync();
    }
}