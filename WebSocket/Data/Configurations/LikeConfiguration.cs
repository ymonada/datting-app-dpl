using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebSocket.Domain.Entity;
using WebSocket.Domain.UserAggregate;

namespace WebSocket.Data.Configurations;

public class LikeConfiguration : IEntityTypeConfiguration<Like>
{
    public void Configure(EntityTypeBuilder<Like> builder)
    {
        builder.HasKey(u=>u.Id);
        builder.Property(u=>u.Message).HasMaxLength(256);
        
        builder.Ignore(u=>u.User);
        
        builder.HasOne(u=>u.User)
            .WithMany(u=>u.ReceiveLikes)
            .HasForeignKey(u=>u.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}