using BountyHuntersBlog.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BountyHuntersBlog.Data.Configurations
{
    public class MissionConfiguration : IEntityTypeConfiguration<Mission>
    {
        public void Configure(EntityTypeBuilder<Mission> builder)
        {
            builder.Property(m => m.CreatedOn)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.HasOne(m => m.User)
                .WithMany(u => u.Missions)
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.Category)
                .WithMany(c => c.Missions)
                .HasForeignKey(m => m.CategoryId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasQueryFilter(m => !m.IsDeleted);
        }
    }
}