using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebSocket.db;
using WebSocket.dto;
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
      await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, result.Data);
      return Ok();
    }
    return Unauthorized(result.Message);

  }
  [HttpPost("register")]
  public async Task<IActionResult> RegisterUser([FromBody] RegisterDto user)
  {
    var result = await _authService.Register(user);
    if (!result.IsSuccess) return BadRequest(result.Message);
    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, result.Data);
    return Ok();
  }
  [HttpPost("logout")]
  public async Task<IActionResult> Logout()
  {
    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    return Ok(new { message = "Logout successful" });
  }
  [HttpGet("check")]
  public IActionResult CheckAuth()
  {
    if (User.Identity?.IsAuthenticated == true)
    {
      return Ok(new { isAuthenticated = true });
    }
    return Unauthorized(new { isAuthenticated = false });
  }
}

