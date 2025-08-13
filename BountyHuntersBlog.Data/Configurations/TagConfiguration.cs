using BountyHuntersBlog.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BountyHuntersBlog.Data.Configurations
{
    public class TagConfiguration : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder.Property(t => t.CreatedOn)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.HasIndex(t => t.Name)
                .IsUnique();

            builder.HasQueryFilter(t => !t.IsDeleted);
        }
    }
}