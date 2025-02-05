using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebSocket.Entity;

namespace WebSocket.db.Configurations;

public class LikeConfiguration : IEntityTypeConfiguration<Like>
{
    public void Configure(EntityTypeBuilder<Like> builder)
    {
        builder.HasKey(u=>u.Id);
        builder.Property(u => u.Id).ValueGeneratedOnAdd();
        builder.HasOne(u => u.UserTo)
            .WithMany(u => u.ReceiveLikes)
            .HasForeignKey(u=>u.UserToId)
            .OnDelete(DeleteBehavior.Cascade);
        // builder.HasOne(u => u.UserFrom)
        //     .WithMany(u => u.SendLikes)
        //     .HasForeignKey(u => u.UserFromId)
        //     .OnDelete(DeleteBehavior.Cascade);

    }
}