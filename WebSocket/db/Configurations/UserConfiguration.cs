using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebSocket.Entity;

namespace WebSocket.db.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id).ValueGeneratedOnAdd();
        builder.Property(u => u.IsActive);
        builder.Property(u => u.Bio)
            .HasMaxLength(1024);
        builder.Property(u => u.Age);
        builder.Property(u => u.Gender);
        builder.Property(u => u.GenderPreference);
        builder.Property(u => u.Name)
            .HasMaxLength(64);
        builder.Property(u => u.PasswordHash);
        builder.Property(u => u.Email)
            .HasMaxLength(128);
        builder.Property(u => u.City);

        builder.HasMany(u => u.Roles)
            .WithOne(u => u.User)
            .HasForeignKey(u => u.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.Photos)
            .WithOne(u => u.User)
            .HasForeignKey(u => u.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.ReceiveLikes)
            .WithOne(u => u.UserTo)
            .HasForeignKey(u => u.UserToId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
