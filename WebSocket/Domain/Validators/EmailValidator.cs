using FluentValidation;
using Microsoft.EntityFrameworkCore;
using WebSocket.db;
using WebSocket.Domain.Entity;
using WebSocket.Domain.UserAggregate;
using WebSocket.Domain.ValueObjects;

namespace WebSocket.Domain.Validators;

public class EmailValidator : AbstractValidator<Email>
{
    public EmailValidator(AppDbContext dbContext)
    {
        RuleFor(email => email.Value)
            .EmailAddress()
            .WithMessage("Invalid email address")
            .MustAsync(async (email, ctx) =>
                !await dbContext.Users.AnyAsync(c => c.Email.Value == email, ctx
                )
            ).WithMessage("Email address is already exist");
        // .MustAsync(async (email, ctx) => 
        //     !await dbContext.BlockedLists
        //         .AnyAsync(x=> x.Email.Value==email, ctx))
        // .WithMessage("Email is blocked");
    }
}