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

            // Many-to-many: MissionPost <-> Faction
            builder.Entity<MissionPost>()

                .HasOne(mp => mp.Author)
                .WithMany(h => h.Missions)
                .HasForeignKey(mp => mp.AuthorId)
                .IsRequired(false) // ако авторът може да бъде null
                .OnDelete(DeleteBehavior.SetNull); // или DeleteBehavior.Restrict/Cascade


                .HasMany(m => m.Factions)
                .WithMany(f => f.MissionPosts);


            // Optional: Config for MissionLike (composite key if needed)
            builder.Entity<MissionLike>()
                .HasKey(l => new { l.MissionPostId, l.UserId });

            builder.Entity<MissionComment>()
                .HasOne(c => c.MissionPost)
                .WithMany(m => m.Comments)
                .HasForeignKey(c => c.MissionPostId);


            // Optional: If using custom Identity user (Hunter), configure here
            builder.Entity<Hunter>()
                .HasMany(h => h.Missions)
                .WithOne()
                .HasForeignKey(m => m.AuthorId)
                .OnDelete(DeleteBehavior.SetNull);

        }
    }
}