using System.Security.Claims;
using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebSocket.Contracts.User;
using WebSocket.Domain.Enums;
using WebSocket.Domain.UserAggregate;

namespace WebSocket.Features.User;

[ApiController]
[Route("[controller]")]
public class UserController(UserService userService,
  ILogger<UserController> logger)
  : ControllerBase
{
    private Guid GetId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException());

    [HttpGet("/profile")]
    [Authorize]
    public async Task<IActionResult> GetUserProfile(CancellationToken ct)
    {
        var userId = GetId();
        
        var result = await userService.User(userId, ct);
        return result.Match<IActionResult>(
                v=> Ok(v),
                err=> BadRequest(err)
        );
    }
    
    [HttpPost("/sendLike")]
    [Authorize]
    public async Task<IActionResult> SendLike(List<UserService.SendLikeRequestDto> sendLikeRequestDto, CancellationToken ct)
    {
        // var userId = GetId();
        var result = await userService.SendLikeToUsers(GetId(), sendLikeRequestDto, ct);
        return result.Match<IActionResult>(
            v => Ok(v),
            err => BadRequest(err));
    }

    [HttpPut("/userUpdate")]
    [Authorize]
    public async Task<IActionResult> UpdateUser([FromForm] UserUpdateRequestDto data, CancellationToken ct)
    {
        // if (data == null) return BadRequest(Error.Validation(description: "Request data is null"));
        var userId = GetId();
        if (data.Photos.Count != 0)
        {
            foreach (var photo in data.Photos)
            {
                var badRequestObjectResult = photo.Length switch
                {
                    0 => BadRequest(Error.Validation(description: "One or more photos are invalid")),
                    > 10 * 1024 * 1024 => BadRequest(Error.Validation(description: "Photo size exceeds 10MB")),
                    _ => BadRequest(Error.Validation(description: "Incorrect data"))
                };
                
            }
        }

        var result = await userService.UpdateProfile(userId, data, ct);
        return result.Match<IActionResult>(
            v => Ok(v),
            err => BadRequest(err)
        );
    }
    
}
