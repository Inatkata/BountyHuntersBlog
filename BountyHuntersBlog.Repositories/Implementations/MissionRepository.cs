using BountyHuntersBlog.Data;
using BountyHuntersBlog.Data.Models;
using BountyHuntersBlog.Repositories.Base;
using BountyHuntersBlog.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BountyHuntersBlog.Repositories.Implementations
{
    public class MissionRepository
        : Repository<Mission>, IMissionRepository
    {
        private readonly BountyHuntersDbContext _db;

        public MissionRepository(BountyHuntersDbContext context)
            : base(context)
        {
            _db = context;
        }

        public async Task<IEnumerable<Mission>> GetByAuthorAsync(string authorId)
            => await Context.Missions
                .Include(m => m.Author)
                .Where(m => m.AuthorId == authorId)
                .ToListAsync();

        public async Task<bool> ExistsAsync(int id)
            => await Context.Missions.AnyAsync(m => m.Id == id);
    }
}