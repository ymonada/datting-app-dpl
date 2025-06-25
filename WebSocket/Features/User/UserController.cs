using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebSocket.dto;

namespace WebSocket.Features.User;

[ApiController]
[Route("[controller]")]
public class UserController(UserService userService,
  ILogger<UserController> logger)
  : ControllerBase
{

    private int GetId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");

    [HttpGet("/api/v2/profile")]
    [Authorize]
    public async Task<IActionResult> GetUserProfile(CancellationToken ct)
    {
        var userId = GetId();
        if (userId == 0)
            throw new UnauthorizedAccessException();
        var result = await userService.GetUserInfo(userId, ct);

        return result
          .ForEachError(x => logger.LogError(x.Description))
          .Match<IActionResult>(
          v => Ok(v),
          err =>
          {
              logger.LogError(err.Description);
              return BadRequest(err.Description);
          }
        );
    }

    [HttpPost("/setActive")]
    [Authorize]
    public async Task<IActionResult> SetProfileActive()
    {
        await userService.SetActiveAll();
        return Ok();
    }
    [HttpGet("/find")]
    public async Task<IActionResult> Find()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var result = await userService.FindProfileAsync(userId);
        return result.ForEachError(x=>logger.LogError(x.Description??"Error loading profile"))
            .Match<IActionResult>(
                v => Ok(v),
                err => BadRequest(err.Description));
    }

    [HttpPut("/updateProfile")]
    [Authorize]
    public async Task<IActionResult> UpdateProfile(UserUpdateProfileDto newProfile, CancellationToken ct)
    {
        var userId = GetId();
        var result = await userService.UpdateMyProfile(userId, newProfile, ct);
        return result
            .ForEachError(x => logger.LogError(x.Description))
            .Match<IActionResult>(
                v => Ok(v),
                err => BadRequest(err.Description));
    }

}
