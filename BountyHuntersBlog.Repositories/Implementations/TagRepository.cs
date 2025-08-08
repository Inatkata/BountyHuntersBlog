using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BountyHuntersBlog.Data;
using BountyHuntersBlog.Data.Models;
using BountyHuntersBlog.Repositories.Base;
using BountyHuntersBlog.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BountyHuntersBlog.Repositories.Implementations
{
    public class TagRepository : Repository<Tag>, ITagRepository
    {
        public TagRepository(BountyHuntersDbContext context)
            : base(context) { }
        public async Task<Tag?> GetByNameAsync(string name)
            => await DbSet.FirstOrDefaultAsync(t => t.Name == name);
        public async Task<IEnumerable<Tag>> GetTagsByMissionIdAsync(int missionId)
            => await Context.MissionTags
                .Where(mt => mt.MissionId == missionId)
                .Select(mt => mt.Tag)
                .ToListAsync();
    }
}
