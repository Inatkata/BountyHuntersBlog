using BountyHuntersBlog.Models.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BountyHuntersBlog.Data
{
    public class BountyHuntersDbContext : IdentityDbContext<Hunter>
    {
        public BountyHuntersDbContext(DbContextOptions<BountyHuntersDbContext> options)
            : base(options)
        {
        }

        public DbSet<MissionPost> MissionPosts { get; set; }
        public DbSet<Faction> Factions { get; set; }
        public DbSet<MissionLike> MissionLikes { get; set; }
        public DbSet<MissionComment> MissionComments { get; set; }

        // Ако има други ентитети, добави ги тук:
        // public DbSet<SomeEntity> SomeEntities { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Пример за допълнителна конфигурация (ако има нужда):
            builder.Entity<MissionLike>()
                .HasKey(x => new { x.MissionPostId, x.HunterId });

            builder.Entity<MissionComment>()
                .HasOne(x => x.Hunter)
                .WithMany(x => x.MissionComments)
                .HasForeignKey(x => x.HunterId);
        }
    }
}