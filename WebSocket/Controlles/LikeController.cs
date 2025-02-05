using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using WebSocket.db;
using WebSocket.Entity;
using WebSocket.Service;

namespace WebSocket.Controlles;


[ApiController]
[Route("[controller]")]
public class LikeController : ControllerBase
{
    private readonly LikeService _likeService;

    public LikeController(LikeService likeService)
    {
        _likeService = likeService;
    }
    [HttpPost("/sendLike")]
    [Authorize]
    public async Task<IActionResult> SendLike(int userFollower)
    { 
        var userSenderId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userSenderId != null)
        {
            var result = await _likeService.SendLike(int.Parse(userSenderId), userFollower);
            if(result.IsSuccess) 
                return Ok(result.Data);
        }
        return BadRequest();
    }
    [HttpPost("/sendDislike")]
    [Authorize]
    public async Task<IActionResult> SendDislike([FromBody] int idUserDisliked)
    { 
        var userSenderId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userSenderId == null)
            return BadRequest();
        
        await _likeService.SendDislike(int.Parse(userSenderId), idUserDisliked);
        return Ok();
    }
    [HttpGet("/mylikes")]
    [Authorize]
    public async Task<IActionResult> GetMyLikes()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result = await _likeService.GetLikes(int.Parse(userId));
        if (result.IsSuccess)
        {
            return Ok(result.Data);
        }
        return Ok(result.Message);
    }

    [HttpPost("/dislikeResponse")]
    [Authorize]
    public async Task<IActionResult> SendDislikeResponse([FromBody] int userDislikedId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result = await _likeService.SendDislikeResponse(int.Parse(userId), userDislikedId);
        if (result.IsSuccess)
        {
            return Ok();
        }
        return BadRequest(result.Message);
    }
    [HttpPost("/likeResponse")]
    [Authorize]
    public async Task<IActionResult> SendLikeResponse([FromBody] int userLikedId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result = await _likeService.SendLikeResponse(int.Parse(userId), userLikedId);
        if (result.IsSuccess)
        {
            return Ok();
        }
        return BadRequest(result.Message);
    }
  
}
