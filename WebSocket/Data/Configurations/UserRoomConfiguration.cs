using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebSocket.Domain.RoomAggregate;

namespace WebSocket.Data.Configurations;

public class UserRoomConfiguration : IEntityTypeConfiguration<UserRoom>
{
    public void Configure(EntityTypeBuilder<UserRoom> builder)
    {
        builder.HasKey(u => u.Id);
        builder.HasOne(u => u.User)
            .WithMany(u => u.Rooms)
            .HasForeignKey(u => u.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(u => u.Room)
            .WithMany(u => u.UserRooms)
            .HasForeignKey(u => u.RoomId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(u=>u.Messages)
            .WithOne(m => m.UserRoom)
            .HasForeignKey(m => m.UserRoomId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}