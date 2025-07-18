using FluentValidation;
using WebSocket.Domain.ValueObjects;

namespace WebSocket.Domain.Validators;

public class CredentialsValidator : AbstractValidator<Credentials>
{
    private const byte MinLength = 5;
    public CredentialsValidator()
    {
        RuleFor(x=> x.PasswordHash)
            .NotEmpty()
            .WithMessage("Password is required")
            .MinimumLength(MinLength)
            .WithMessage($"Password must be at least {MinLength} characters long");
    }
}