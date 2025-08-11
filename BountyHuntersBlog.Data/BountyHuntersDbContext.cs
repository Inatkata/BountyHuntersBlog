using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using BountyHuntersBlog.Data.Models;

namespace BountyHuntersBlog.Data
{
    public class BountyHuntersDbContext : IdentityDbContext<ApplicationUser>
    {
        public BountyHuntersDbContext(DbContextOptions<BountyHuntersDbContext> options)
            : base(options) { }

        public DbSet<Mission> Missions => Set<Mission>();
        public DbSet<Comment> Comments => Set<Comment>();
        public DbSet<Like> Likes => Set<Like>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Tag> Tags => Set<Tag>();
        public DbSet<MissionTag> MissionTags => Set<MissionTag>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Apply all IEntityTypeConfiguration<T> in this assembly
            builder.ApplyConfigurationsFromAssembly(typeof(BountyHuntersDbContext).Assembly);

            // Global query filters for soft-delete
            builder.Entity<Mission>().HasQueryFilter(m => !m.IsDeleted);
            builder.Entity<Category>().HasQueryFilter(c => !c.IsDeleted);
            builder.Entity<Tag>().HasQueryFilter(t => !t.IsDeleted);
        }
    }
}