// BountyHuntersBlog.Data/Configurations/CategoryConfiguration.cs
using BountyHuntersBlog.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BountyHuntersBlog.Data.Constants;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> b)
    {
        b.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(ModelConstants.User.DisplayNameMaxLength);

        b.Property(x => x.IsDeleted).HasDefaultValue(false);
        b.Property(x => x.CreatedOn).HasDefaultValueSql("GETUTCDATE()");

        // Уникален индекс за име (case-insensitive при SQL Server collation)
        b.HasIndex(x => x.Name).IsUnique();

        // Ако не искаш твърда уникалност, а soft-delete:
        // b.HasIndex(x => new { x.Name, x.IsDeleted }).IsUnique();

        // (по избор) Тригериране на ModifiedOn през save interceptor или ръчно в service слоя.
    }
}