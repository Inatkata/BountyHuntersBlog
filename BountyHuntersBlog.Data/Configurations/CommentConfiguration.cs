using BountyHuntersBlog.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> b)
    {
        b.Property(x => x.CreatedOn).HasDefaultValueSql("GETUTCDATE()");
        b.Property(x => x.Text).HasMaxLength(4000).IsRequired();

        b.HasOne(c => c.Mission)
            .WithMany(m => m.Comments)
            .HasForeignKey(c => c.MissionId)
            .OnDelete(DeleteBehavior.Cascade);

        b.HasOne(c => c.User)
            .WithMany(u => u.Comments)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}