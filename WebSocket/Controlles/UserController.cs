using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebSocket.db;
using WebSocket.Entity;
using WebSocket.dto;
using WebSocket.Service;

namespace WebSocket.Controlles;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
  private readonly UserService _userService;

  public UserController(UserService userService)
  {
    _userService = userService;
  }

  [HttpGet("/profile")]
  [Authorize]
  public async Task<IActionResult> GetUserProfile()
  {
    var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
    var result = await _userService.GetUserInfo(userId);
    if (result.IsSuccess)
    {
      return Ok(result.Data);
    }
    return BadRequest(result.Message);
  }

  [HttpPost("/setActive")]
  [Authorize]
  public async Task<IActionResult> SetProfileActive()
  {
    await _userService.SetActiveAll();
    return Ok();
  }
  [HttpGet("/find")]
  public async Task<IActionResult> Find()
  {
    var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
    var result = await _userService.Finding(userId);
    if (result.IsSuccess)
    {
      return Ok(result.Data);
    }
    return BadRequest(result.Message);
  }

  [HttpPut("/updateProfile")]
  [Authorize]
  public async Task<IActionResult> UpdateProfile(UserUpdateProfileDto newProfile)
  {
    var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
    var result = await _userService.UpdateMyProfile(userId, newProfile);
    if (result.IsSuccess)
      return Ok(result.Message);
    return BadRequest(result.Message);
  }

}
