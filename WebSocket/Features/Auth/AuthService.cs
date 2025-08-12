using System.Runtime.InteropServices.JavaScript;
using System.Security.Claims;
using ErrorOr;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using WebSocket.db;
using WebSocket.Domain.dto;
using WebSocket.Domain.Entity;
using WebSocket.Domain.ValueObjects;
using WebSocket.dto;
using WebSocket.Features.Auth;
using WebSocket.Features.User;

namespace WebSocket.Service;

public class AuthService
{
    private readonly AppDbContext _context;
    private readonly TokenService _tokenService;

    public AuthService(AppDbContext context
        , TokenService tokenService
        , UserService userService)
    {
        _context = context;
        _tokenService = tokenService;
    }
    public async Task<ErrorOr<ClaimsPrincipal>> Login(LoginDto userLogin)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email.Value == userLogin.Email);

        if (user is null) return Error.NotFound(description:"User not found");
        
        if (!Credentials.PasswordVerify(userLogin.Password, user.Credentials.PasswordHash))
            return Error.Failure(description:"Email or password is incorrect");
        
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };
        
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        return new ClaimsPrincipal(identity);
    }
}
