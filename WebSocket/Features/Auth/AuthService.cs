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

    // public async Task<ServiceResult<ClaimsPrincipal>> Register(RegisterDto userRegister)
    // {
    //     var user = await _context.UserRooms.FirstOrDefaultAsync(u => u.Email == userRegister.Email);
    //     if (user != null)
    //     {
    //         return new ServiceResult<ClaimsPrincipal>
    //         {
    //             IsSuccess = false,
    //             Message = "User already exists!"
    //         };
    //     }
    //     var defaultRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Member");
    //     if (defaultRole == null)
    //     {
    //         await _context.Roles.AddAsync(new Role { Name = "Member" });
    //         await _context.SaveChangesAsync();
    //     }
    //     defaultRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Member");
    //
    //
    //     var newUser = new User
    //     {
    //         Name = userRegister.Name
    //         ,
    //         PasswordHash = HashPassword(userRegister.Password)
    //         ,
    //         Email = userRegister.Email
    //         ,
    //         Roles = new List<UserRole>
    //         {
    //             new ()
    //             {
    //                 RoleId = defaultRole.Id
    //             }
    //         }
    //     };
    //     await _context.UserRooms.AddAsync(newUser);
    //     await _context.SaveChangesAsync();
    //     var claims = new List<Claim>
    //     {
    //         new Claim(ClaimTypes.NameIdentifier, newUser.Id.ToString())
    //     };
    //     claims.AddRange(newUser.Roles.Select(r => new Claim(ClaimTypes.Role, r.Role.Name)));
    //
    //     var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
    //     var principal = new ClaimsPrincipal(identity);
    //     //var token = _tokenService.GenerateToken(GetUserTokenDto(newUser));
    //     return new ServiceResult<ClaimsPrincipal>
    //     {
    //         IsSuccess = true,
    //         Data = principal,
    //         Message = "User created!"
    //     };
    // }
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
