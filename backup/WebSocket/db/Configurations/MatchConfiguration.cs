using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebSocket.Entity;

namespace WebSocket.db.Configurations;

public class MatchConfiguration: IEntityTypeConfiguration<Match>
{
    public void Configure(EntityTypeBuilder<Match> builder)
    {
        builder.HasIndex(m => new { m.FirstUserId, m.SecondUserId }).IsUnique();
        
        builder.HasOne(u=>u.FirstUser)
            .WithMany(u=>u.Matches)
            .HasForeignKey(u=>u.FirstUserId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(u=>u.SecondUser)
            .WithMany()
            .HasForeignKey(u=>u.SecondUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}