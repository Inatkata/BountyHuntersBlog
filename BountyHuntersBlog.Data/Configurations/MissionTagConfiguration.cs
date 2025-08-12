// BountyHuntersBlog.Data/Configurations/MissionTagConfiguration.cs
using BountyHuntersBlog.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class MissionTagConfiguration : IEntityTypeConfiguration<MissionTag>
{
    public void Configure(EntityTypeBuilder<MissionTag> b)
    {
        // Композитен ключ: предотвратява дублиране на (MissionId, TagId)
        b.HasKey(mt => new { mt.MissionId, mt.TagId });

        b.Property(mt => mt.CreatedOn)
            .HasDefaultValueSql("GETUTCDATE()");

        b.HasOne(mt => mt.Mission)
            .WithMany(m => m.MissionTags)
            .HasForeignKey(mt => mt.MissionId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(mt => mt.Tag)
            .WithMany(t => t.MissionTags)
            .HasForeignKey(mt => mt.TagId)
            .OnDelete(DeleteBehavior.Restrict);

        // Полезни индекси за филтър
        b.HasQueryFilter(mt => !mt.Mission.IsDeleted);
        b.HasIndex(mt => mt.TagId);
        b.HasIndex(mt => mt.MissionId);
    }
}