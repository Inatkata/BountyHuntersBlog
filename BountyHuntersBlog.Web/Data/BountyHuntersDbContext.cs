using BountyHuntersBlog.Models.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BountyHuntersBlog.Data
{
    public class BountyHuntersDbContext : IdentityDbContext
    {
        public BountyHuntersDbContext(DbContextOptions<BountyHuntersDbContext> options)
            : base(options)
        {
        }

        public DbSet<MissionPost> MissionPosts { get; set; }
        public DbSet<Faction> Factions { get; set; }
        public DbSet<MissionLike> MissionLikes { get; set; }
        public DbSet<MissionComment> MissionComments { get; set; }
        public DbSet<Hunter> Hunters { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // One-to-Many: MissionPost <-> Hunter (Author)
            builder.Entity<MissionPost>()
                .HasOne(mp => mp.Author)
                .WithMany(h => h.Missions)
                .HasForeignKey(mp => mp.AuthorId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            // Many-to-Many: MissionPost <-> Faction
            builder.Entity<MissionPost>()
                .HasMany(m => m.Factions)
                .WithMany(f => f.MissionPosts);

            // Composite Key: MissionLike
            builder.Entity<MissionLike>()
                .HasKey(l => new { l.MissionPostId, l.UserId });

            // One-to-Many: MissionPost <-> MissionComment
            builder.Entity<MissionComment>()
                .HasOne(c => c.MissionPost)
                .WithMany(m => m.Comments)
                .HasForeignKey(c => c.MissionPostId);

            // One-to-Many (optional): Hunter <-> Missions
            builder.Entity<Hunter>()
                .HasMany(h => h.Missions)
                .WithOne()
                .HasForeignKey(m => m.AuthorId)
                .OnDelete(DeleteBehavior.SetNull);
        }

    }
}