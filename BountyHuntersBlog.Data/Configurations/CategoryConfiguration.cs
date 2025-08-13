using BountyHuntersBlog.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BountyHuntersBlog.Data.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.Property(c => c.CreatedOn)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.HasIndex(c => c.Name)
                .IsUnique();

            builder.HasQueryFilter(c => !c.IsDeleted);
        }
    }
}