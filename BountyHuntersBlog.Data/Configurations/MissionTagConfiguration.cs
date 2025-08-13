using BountyHuntersBlog.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BountyHuntersBlog.Data.Configurations
{
    public class MissionTagConfiguration : IEntityTypeConfiguration<MissionTag>
    {
        public void Configure(EntityTypeBuilder<MissionTag> builder)
        {
            builder.HasKey(mt => new { mt.MissionId, mt.TagId });

            builder.Property(mt => mt.CreatedOn)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.HasOne(mt => mt.Mission)
                .WithMany(m => m.MissionTags)
                .HasForeignKey(mt => mt.MissionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(mt => mt.Tag)
                .WithMany(t => t.MissionTags)
                .HasForeignKey(mt => mt.TagId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}