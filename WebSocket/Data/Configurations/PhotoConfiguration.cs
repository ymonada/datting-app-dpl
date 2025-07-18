using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebSocket.Domain.Entity;

namespace WebSocket.Data.Configurations;

public class PhotoConfiguration : IEntityTypeConfiguration<Photo>
{
    public void Configure(EntityTypeBuilder<Photo> builder)
    {
        builder.HasKey(u=>u.Id);
        builder.Property(u => u.Id)
            .ValueGeneratedOnAdd();
        builder.Property(u => u.Url);
        
        builder.HasOne(u=>u.User)
            .WithMany(u=>u.Photos)
            .HasForeignKey(u=>u.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}