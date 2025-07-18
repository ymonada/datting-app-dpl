using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using FluentValidation;
using WebSocket.Domain.Entity;
using WebSocket.Domain.UserAggregate;

namespace WebSocket.Domain.Validators;

public abstract class UserValidation : AbstractValidator<User> 
{
    public UserValidation()
    {
        
    }
    
}