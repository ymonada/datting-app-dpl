using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace WebSocket.Extensions;

public static class AddAuthExtension
{
    public static IServiceCollection AddAuth(this IServiceCollection services)
    {
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(o => o.Cookie.Name = "moloko");
        
        services.AddAuthorization();
        
        return services;
    }
}
