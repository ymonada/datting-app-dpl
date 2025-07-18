using System.Security.Authentication;
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
    private Guid GetId() => Guid.Parse((ReadOnlySpan<char>)(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException()));

    // [HttpGet("/api/v2/profile")]
    // [Authorize]
    // public async Task<IActionResult> GetUserProfile(CancellationToken ct)
    // {
    //     var userId = GetId();
    //     var result = await userService.(userId, ct);
    //
    //     return result
    //       .ForEachError(x => logger.LogError(x.Description))
    //       .Match<IActionResult>(
    //       v => Ok(v),
    //       err =>
    //       {
    //           logger.LogError(err.Description);
    //           return BadRequest(err.Description);
    //       }
    //     );
    // }
    
    
    
    [HttpPost("/sendLike")]
    [Authorize]
    public async Task<IActionResult> SendLike(UserService.LikeRequest likeRequest, CancellationToken ct)
    {
        // var userId = GetId();
        var result = await userService.SendLike(likeRequest, ct);
        return result.Match<IActionResult>(
            v => Ok(v),
            err => BadRequest(err));
    }

}
