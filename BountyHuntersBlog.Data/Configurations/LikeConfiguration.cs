using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BountyHuntersBlog.Data.Models;

namespace BountyHuntersBlog.Data.Configurations
{
    public class LikeConfiguration : IEntityTypeConfiguration<Like>
    {
        public void Configure(EntityTypeBuilder<Like> builder)
        {
            builder.HasKey(l => l.Id);

            builder.Property(l => l.CreatedOn)
                .HasDefaultValueSql("GETDATE()");

            builder.HasOne(l => l.User)
                .WithMany(u => u.Likes)
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(l => l.Mission)
                .WithMany(m => m.Likes)
                .HasForeignKey(l => l.MissionId);

            builder.HasOne(l => l.Comment)
                .WithMany(c => c.Likes)
                .HasForeignKey(l => l.CommentId);
        }
    }
}