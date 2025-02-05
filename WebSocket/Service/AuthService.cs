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
    public async Task<ServiceResult<string>> Register(RegisterDto userRegister)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userRegister.Email);
        var defaultRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Member");
        if(user != null || defaultRole == null)
        {
            return new ServiceResult<string>
            {
                IsSuccess = false,
                Message = "User already exists!"
            };
        }

        var newUser = new User
        {
            Name = userRegister.Name
            , PasswordHash = HashPassword(userRegister.Password)
            , Email = userRegister.Email
            , Roles = new List<UserRole>
            {
                new ()
                {
                    RoleId = defaultRole.Id
                }
            }
        };
        await _context.Users.AddAsync(newUser);
        await _context.SaveChangesAsync();
        var token = _tokenService.GenerateToken(GetUserTokenDto(newUser));
        return new ServiceResult<string>
        {
            IsSuccess = true,
            Data = token,
            Message = "User created!"
        };
    }
    public async Task<ServiceResult<string>> Login(LoginDto userLogin)
    {
        var user = await _context.Users
            .Include(user => user.Roles)
            .ThenInclude(userRole => userRole.Role)
            .FirstOrDefaultAsync(u => u.Email == userLogin.Email);
        
        if (user == null)
        {
            return new ServiceResult<string>
            {
                IsSuccess = false,
                Message = "Email or password is incorrect"
            };
        }
        if (!PasswordVerify(userLogin.Password, user.PasswordHash))
        {
            return new ServiceResult<string>
            {
                IsSuccess = false,
                Message = "Email or password is incorrect"
            };
        }
        user.LastActiveTime = DateTime.UtcNow;
        
        var token = _tokenService.GenerateToken(GetUserTokenDto(user));
        user.LastActiveTime = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return new ServiceResult<string>
        {
            IsSuccess = true,
            Message = "ok", 
            Data = token
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