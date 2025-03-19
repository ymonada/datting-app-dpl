using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using WebSocket.db;
using WebSocket.dto;
using WebSocket.Entity;

namespace WebSocket.Service;

public class AuthService
{
    private readonly AppDbContext _context;
    private readonly TokenService _tokenService;

    public AuthService(AppDbContext context
        , TokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }
    public async Task<ServiceResult<ClaimsPrincipal>> Register(RegisterDto userRegister)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userRegister.Email);
        if (user != null)
        {
            return new ServiceResult<ClaimsPrincipal>
            {
                IsSuccess = false,
                Message = "User already exists!"
            };
        }
        var defaultRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Member");
        if (defaultRole == null)
        {
            await _context.Roles.AddAsync(new Role { Name = "Member" });
            await _context.SaveChangesAsync();
        }
        defaultRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Member");


        var newUser = new User
        {
            Name = userRegister.Name
            ,
            PasswordHash = HashPassword(userRegister.Password)
            ,
            Email = userRegister.Email
            ,
            Roles = new List<UserRole>
            {
                new ()
                {
                    RoleId = defaultRole.Id
                }
            }
        };
        await _context.Users.AddAsync(newUser);
        await _context.SaveChangesAsync();
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, newUser.Id.ToString())
        };
        claims.AddRange(newUser.Roles.Select(r => new Claim(ClaimTypes.Role, r.Role.Name)));

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);
        //var token = _tokenService.GenerateToken(GetUserTokenDto(newUser));
        return new ServiceResult<ClaimsPrincipal>
        {
            IsSuccess = true,
            Data = principal,
            Message = "User created!"
        };
    }
    public async Task<ServiceResult<ClaimsPrincipal>> Login(LoginDto userLogin)
    {
        var user = await _context.Users
            .Include(user => user.Roles)
            .ThenInclude(userRole => userRole.Role)
            .FirstOrDefaultAsync(u => u.Email == userLogin.Email);

        if (user == null)
        {
            return new ServiceResult<ClaimsPrincipal>
            {
                IsSuccess = false,
                Message = "Email or password is incorrect"
            };
        }
        if (!PasswordVerify(userLogin.Password, user.PasswordHash))
        {
            return new ServiceResult<ClaimsPrincipal>
            {
                IsSuccess = false,
                Message = "Email or password is incorrect"
            };
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };
        claims.AddRange(user.Roles.Select(r => new Claim(ClaimTypes.Role, r.Role.Name)));

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);


        //var token = _tokenService.GenerateToken(GetUserTokenDto(user));

        user.LastActiveTime = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return new ServiceResult<ClaimsPrincipal>
        {
            IsSuccess = true,
            Message = "ok",
            Data = principal
        };
    }


    private bool PasswordVerify(string inputPassword, string passwordHash)
    {
        return BCrypt.Net.BCrypt.Verify(inputPassword, passwordHash);
    }
    private string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    private UserTokenDto GetUserTokenDto(User user)
    {
        return new UserTokenDto(
            user.Id,
            user.Email,
            user.Roles
                .Select(userRole => new RoleDto(userRole.RoleId, userRole.Role.Name)));

    }

}
