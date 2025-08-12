using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebSocket.Domain.Common;
using WebSocket.Domain.UserAggregate;

namespace WebSocket.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);
    
        builder.ComplexProperty(c => c.Email, email =>
        {
            email.Property(u=>u.Value)
                .HasMaxLength(256)
                .HasColumnName("Email")
                .IsRequired();
            email.Property(u=>u.Status)
                .HasColumnName("Email_Status");
        });
        builder.ComplexProperty(c => c.Credentials, c =>
        {
            c.Property(u=>u.PasswordHash)
                .HasColumnName("PasswordHash")
                .IsRequired();
        });
        builder.Property(c => c.AccountStatus).IsRequired();
       
        builder.Property(u => u.CreatedDateTime)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
        builder.Property(c => c.LastActiveDateTime);
 
       builder.HasMany(u => u.Photos)
            .WithOne(u => u.User)
            .HasForeignKey(u => u.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.ReceiveLikes)
            .WithOne(u => u.User)
            .HasForeignKey(u => u.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(u=>u.Profile)
            .WithOne(u=>u.User)
            .HasForeignKey<Profile>(u=>u.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(u=>u.Rooms)
            .WithOne(u=>u.User)
            .HasForeignKey(u=>u.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(u=>u.Messages)
            .WithOne(u=>u.User)
            .HasForeignKey(u=>u.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
