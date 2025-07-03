using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebSocket.Entity;

namespace WebSocket.db.Configurations;

public class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.HasKey(m => m.Id);
        
        builder.Property(m => m.Content)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(m => m.SentAt)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(m => m.IsRead)
            .IsRequired()
            .HasDefaultValue(false);

        // // Зв’язок із Match
        // builder.HasOne(m => m.Match)
        //     .WithMany(m => m.MessagesHistory) // Додаємо колекцію Messages у Match
        //     .HasForeignKey(m => m.MatchId)
        //     .OnDelete(DeleteBehavior.Cascade); // Видаляємо повідомлення, якщо матч видалено

        // Зв’язок із Sender (User)
        builder.HasOne(m => m.Sender)
            .WithMany()
            .HasForeignKey(m => m.SenderId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}