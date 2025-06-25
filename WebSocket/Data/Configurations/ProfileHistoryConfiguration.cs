using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebSocket.Entity;

namespace WebSocket.db.Configurations;

public class ProfileHistoryConfiguration : IEntityTypeConfiguration<ProfileHistory>
{
    public void Configure(EntityTypeBuilder<ProfileHistory> builder)
    {
        builder.HasKey(ur => new { ur.UserId, ur.WatchId });
        
        builder.HasOne(ur => ur.User)
            .WithMany(u=>u.ProfileHistory)
            .HasForeignKey(ur => ur.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}