using Microsoft.EntityFrameworkCore;
using WebSocket.db;

namespace WebSocket.Service;

public class UserActivityCheckerService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public UserActivityCheckerService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var inactiveusers = await context.Users
                    .Where(u => DateTime.UtcNow - u.LastActiveTime > TimeSpan.FromMinutes(3))
                    .ToListAsync(cancellationToken: stoppingToken);
                foreach (var user in inactiveusers)
                {
                    user.IsActive = false;
                }

                if (inactiveusers.Any())
                    await context.SaveChangesAsync(stoppingToken);
            }

            await Task.Delay(TimeSpan.FromMinutes(3), stoppingToken);
            
        }
    }
}