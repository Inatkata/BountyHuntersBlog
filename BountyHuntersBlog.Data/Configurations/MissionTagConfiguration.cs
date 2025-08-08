using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BountyHuntersBlog.Data.Models;

namespace BountyHuntersBlog.Data.Configurations
{
    public class MissionTagConfiguration : IEntityTypeConfiguration<MissionTag>
    {
        public void Configure(EntityTypeBuilder<MissionTag> builder)
        {
            builder.HasKey(mt => new { mt.MissionId, mt.TagId });

            builder.HasOne(mt => mt.Mission)
                .WithMany(m => m.MissionTags)
                .HasForeignKey(mt => mt.MissionId);

            builder.HasOne(mt => mt.Tag)
                .WithMany(t => t.MissionTags)
                .HasForeignKey(mt => mt.TagId);
        }
    }
}