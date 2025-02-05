using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebSocket.db;
using WebSocket.dto;
using WebSocket.Entity;
using WebSocket.Service;

namespace WebSocket.Controlles;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto request)
    {
        var result = await _authService.Login(request);
        if (result.IsSuccess)
        {
            return Ok(result.Data);
        }
        return Unauthorized(result.Message);
        
    }
    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser(RegisterDto user)
    {
        var result = await _authService.Register(user);
        if (result.IsSuccess)
        {
            return Ok(result.Data);
        }
        return BadRequest(result.Message);
    }
    // [HttpPost("logout")]
    // public IActionResult Logout()
    // {
    //     // Припустимо, що токен передано через заголовок Authorization
    //     var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
    //
    //     // Зберегти токен у чорний список (наприклад, Redis)
    //     if (!string.IsNullOrEmpty(token))
    //     {
    //         // Додаємо токен до чорного списку з терміном дії (експірації токена)
    //         _blacklistService.AddTokenToBlacklist(token);
    //     }
    //
    //     return Ok(new { message = "Logged out successfully." });
    // }
}

