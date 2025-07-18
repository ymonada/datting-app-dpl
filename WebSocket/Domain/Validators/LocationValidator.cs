using FluentValidation;
using WebSocket.Domain.Entity;
using WebSocket.Domain.UserAggregate;
using WebSocket.Domain.ValueObjects;

namespace WebSocket.Domain.Validators;

public class LocationValidator : AbstractValidator<Location>
{
    private const byte MinLength = 2;
    
    public LocationValidator()
    {
        RuleFor(location => location.Country)
            .NotEmpty()
            .WithMessage("The country is required.")
            .MinimumLength(MinLength);
        RuleFor(location => location.CityOrRegion)
            .NotEmpty()
            .WithMessage("The City or Region is required.")
            .MinimumLength(MinLength);
        }
}