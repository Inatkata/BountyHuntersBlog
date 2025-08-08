using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BountyHuntersBlog.Data.Models;

public class LikeConfiguration : IEntityTypeConfiguration<Like>
{
    public void Configure(EntityTypeBuilder<Like> b)
    {
        
        b.Property(x => x.CreatedOn).HasDefaultValueSql("GETUTCDATE()");


        b.HasOne(l => l.Mission)
            .WithMany(m => m.Likes)
            .HasForeignKey(l => l.MissionId)
            .OnDelete(DeleteBehavior.NoAction);   

        b.HasOne(l => l.Comment)
            .WithMany(c => c.Likes)
            .HasForeignKey(l => l.CommentId)
            .OnDelete(DeleteBehavior.Cascade);

        b.HasOne(l => l.User)
            .WithMany(u => u.Likes)
            .HasForeignKey(l => l.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Един лайк на потребител към конкретен таргет
        b.HasIndex(x => new { x.UserId, x.MissionId })
            .IsUnique()
            .HasFilter("[MissionId] IS NOT NULL");

        b.HasIndex(x => new { x.UserId, x.CommentId })
            .IsUnique()
            .HasFilter("[CommentId] IS NOT NULL");

        // XOR: към Mission ИЛИ към Comment
        b.ToTable(t => t.HasCheckConstraint("CK_Likes_Target",
            "([MissionId] IS NOT NULL AND [CommentId] IS NULL) OR " +
            "([MissionId] IS NULL AND [CommentId] IS NOT NULL)"));
    }
}