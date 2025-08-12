using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using WebSocket.Domain.UserAggregate;
using WebSocket.Domain.ValueObjects;
using WebSocket.dto;
using WebSocket.Features.User;
using WebSocket.Service;

namespace WebSocket.Features.Auth;

public record RegisterRequest(Email Email, Credentials Credentials);

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;
    private readonly UserService _userService;

    public AuthController(AuthService authService, UserService userService)
    {
      _authService = authService;
      _userService =  userService;
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto request)
    {
      var result = await _authService.Login(request);

      if (result.IsError)
        return BadRequest(result.FirstError);
      
      await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, result.Value); 
      return Ok(new { message = "Login successful" });
      }

    public record RegisterRequest(Email Email, Credentials Credentials);
    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterRequest req, CancellationToken ct)
    {
      var result = await _userService.RegisterUser(req.Email, req.Credentials, ct);
      return result.Match<IActionResult>(Ok,BadRequest);
    }
    
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
      await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
      return Ok(new { message = "Logout successful" });
    }

    [HttpGet("check")]
    public IActionResult CheckAuth() => User.Identity?.IsAuthenticated == true
      ? Ok(new { isAuthenticated = true })
      : Unauthorized(new { isAuthenticated = false });

}

