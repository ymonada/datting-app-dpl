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
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(o=>o.Cookie.Name = "moloko");
        // services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        //     .AddJwtBearer(options =>
        //     {
        //         options.TokenValidationParameters = new TokenValidationParameters
        //         {
        //             ValidateIssuer = true,
        //             ValidateAudience = true,
        //             ValidateLifetime = true,
        //             ValidateIssuerSigningKey = true,
        //             ValidIssuer = "yourIssuer",
        //             ValidAudience = "yourAudience",
        //             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("yourSecretKeyyourSecretKeyyourSecretKeyyourSecretKey"))
        //         };
        //         options.Events = new JwtBearerEvents
        //         {
        //             OnMessageReceived = context =>
        //             {
        //                 var accessToken = context.Request.Query["access_token"];
        //                 var path = context.HttpContext.Request.Path;
        //                 if (!string.IsNullOrEmpty(accessToken) &&
        //                     (path.StartsWithSegments("/chat")))
        //                 {
        //                     context.Token = accessToken;
        //                 }
        //                 return Task.CompletedTask;
        //             }
        //         };
        //     });
        // Додавання авторизації
        services.AddAuthorization();
        // services.AddSwaggerGen(options =>
        // {
        //     options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        //     {
        //         Name = "Authorization",
        //         Type = SecuritySchemeType.Http,
        //         Scheme = "Bearer",
        //         BearerFormat = "JWT",
        //         In = ParameterLocation.Header,
        //         Description = "Введіть 'Bearer' [пробіл] і ваш токен JWT. \n\nПриклад: 'Bearer abc123'"
        //     });
        //
        //     options.AddSecurityRequirement(new OpenApiSecurityRequirement
        //     {
        //         {
        //             new OpenApiSecurityScheme
        //             {
        //                 Reference = new OpenApiReference
        //                 {
        //                     Type = ReferenceType.SecurityScheme,
        //                     Id = "Bearer"
        //                 }
        //             },
        //             Array.Empty<string>()
        //         }
        //     });
        return services;
    }
}