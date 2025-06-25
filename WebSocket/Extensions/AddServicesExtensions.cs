using WebSocket.Features.User;
using WebSocket.Service;

namespace WebSocket.Extensions;

public static class AddServicesExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<TokenService>();
        services.AddScoped<AuthService>();
        services.AddScoped<UserService>();
        services.AddScoped<LikeService>();
        services.AddScoped<PhotoService>();
        services.AddHostedService<UserActivityCheckerService>();
        return services;
    }
}