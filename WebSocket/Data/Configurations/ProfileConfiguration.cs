using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebSocket.Domain.UserAggregate;

namespace WebSocket.Data.Configurations;

public class ProfileConfiguration : IEntityTypeConfiguration<Profile>
{
    public void Configure(EntityTypeBuilder<Profile> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(p => p.Name)
            .HasMaxLength(100);

        builder.Property(p => p.Age);
        builder.Property(p => p.Bio).HasMaxLength(500);
        builder.Property(p => p.Gender);
        builder.Property(p => p.GenderPreference);
        
        builder.ComplexProperty(p => p.Location, location =>
        {
            location.Property(l => l.Country)
                .HasMaxLength(100)
                .HasColumnName("Country");

            location.Property(l => l.CityOrRegion)
                .HasMaxLength(100)
                .HasColumnName("CityOrRegion");;
        });
        
        builder.HasOne(u=>u.User)
            .WithOne(u=>u.Profile)
            .HasForeignKey<Profile>(u=>u.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}