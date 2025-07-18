using FluentValidation;
using Microsoft.AspNetCore.Identity;
using WebSocket.Domain.UserAggregate;
using WebSocket.Domain.ValueObjects;

namespace WebSocket.Domain.Validators;

public static class DependencyInjections
{
    public static void AddValidators(this IServiceCollection services)
    {
        // services.AddValidatorsFromAssemblyContaining<UserValidator>();
        services.AddValidatorsFromAssemblyContaining<CredentialsValidator>();
        services.AddValidatorsFromAssemblyContaining<LocationValidator>();
        services.AddValidatorsFromAssemblyContaining<EmailValidator>();
    }
}