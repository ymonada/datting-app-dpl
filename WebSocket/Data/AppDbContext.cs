using Microsoft.EntityFrameworkCore;
using WebSocket.db.Configurations;
using WebSocket.Domain.Entity;
using WebSocket.Entity;

namespace WebSocket.db;


public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Like> Likes { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<Match> Matches { get; set; }
    public DbSet<ProfileHistory> ProfileHistories { get; set; }
    public DbSet<Photo> Photos { get; set; }
    public DbSet<Message> Messages { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new LikeConfiguration());
        modelBuilder.ApplyConfiguration(new RoleConfiguration());
        modelBuilder.ApplyConfiguration(new UserRoleConfiguration());
        modelBuilder.ApplyConfiguration(new PhotoConfiguration());
        modelBuilder.ApplyConfiguration(new ProfileHistoryConfiguration());
        modelBuilder.ApplyConfiguration(new MatchConfiguration());
    }
}