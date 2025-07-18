using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebSocket.Domain.Entity;
using WebSocket.Domain.RoomAggregate;

namespace WebSocket.Data.Configurations;

public class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.HasKey(m => m.Id);
        builder.ComplexProperty(m => m.Content, msg =>
        {
            msg.Property(c => c.Content)
                .IsRequired();
        });

      
        builder.HasOne(m => m.UserRoom)
            .WithMany(u=>u.Messages)
            .HasForeignKey(m => m.UserRoomId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}