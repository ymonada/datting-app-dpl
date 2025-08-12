using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebSocket.Features.Like;


[ApiController]
[Route("[controller]")]
public class LikeController : ControllerBase
{
  private readonly LikeService _likeService;

  public LikeController(LikeService likeService)
  {
    _likeService = likeService;
  }
  private int GetId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException());
  // [HttpPost("/sendLike")]
  // [Authorize]
  // public async Task<IActionResult> SendLike([FromBody] int userFollower)
  // {
  //   var result = await _likeService.Like(GetId(), userFollower);
  //   return result.Match<IActionResult>(
  //     Ok,
  //     err=> BadRequest(err.Description));
  //  }
  // [HttpPost("/sendDislike")]
  // [Authorize]
  // public async Task<IActionResult> SendDislike([FromBody] int idUserDisliked)
  // {
  //   await _likeService.Pass(GetId(), idUserDisliked);
  //   return Ok();
  // }
  // [HttpGet("/mylikes")]
  // [Authorize]
  // public async Task<IActionResult> GetMyLikes()
  // {
  //   var result = await _likeService.GetLikes(GetId());
  //   return result.Match<IActionResult>(Ok,
  //     err=> BadRequest(err.Description));
  // }
  //
  // [HttpPost("/dislikeResponse")]
  // [Authorize]
  // public async Task<IActionResult> SendDislikeResponse([FromBody] int userDislikedId)
  // {
  //   var result = await _likeService.PassResponse(GetId(), userDislikedId);
  //   return result.Match<IActionResult>(Ok,
  //     err=> BadRequest(err.Description));
  // }
  //
  // [HttpPost("/likeResponse")]
  // [Authorize]
  // public async Task<IActionResult> SendLikeResponse([FromBody] int userLikedId)
  // {
  //   var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
  //   var result = await _likeService.LikeResponseDto(GetId(), userLikedId);
  //   return result.Match<IActionResult>(Ok
  //     , err => BadRequest(err.Description));
  // }
}
