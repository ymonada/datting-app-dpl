using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using WebSocket.Entity;
using WebSocket.Service;

namespace WebSocket.Hubs;

[Authorize]
public class FinderHub : Hub
{
   private readonly LikeService _likeService;

    public FinderHub(LikeService likeService)
    {
        _likeService = likeService;
    }

    public async Task SendLike(int userFollower)
    {
        var userSenderId = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userSenderId == null)
        {
            await Clients.Caller.SendAsync("Error", "User not authenticated.");
            return;
        }

        var result = await _likeService.SendLike(int.Parse(userSenderId), userFollower);
        if (result.IsSuccess)
        {
            await Clients.Caller.SendAsync("LikeSent", result.Data);
        }
        else
        {
            await Clients.Caller.SendAsync("Error", "Failed to send like.");
        }
    }

    public async Task SendDislike(int userDislikedId)
    {
        var userSenderId = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userSenderId == null)
        {
            await Clients.Caller.SendAsync("Error", "User not authenticated.");
            return;
        }

        await _likeService.SendDislike(int.Parse(userSenderId), userDislikedId);
        await Clients.Caller.SendAsync("DislikeSent", userDislikedId);
    }

    public async Task GetMyLikes()
    {
        var userId = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            await Clients.Caller.SendAsync("Error", "User not authenticated.");
            return;
        }

        var result = await _likeService.GetLikes(int.Parse(userId));
        if (result.IsSuccess)
        {
            await Clients.Caller.SendAsync("MyLikes", result.Data);
        }
        else
        {
            await Clients.Caller.SendAsync("Error", result.Message);
        }
    }

    public async Task SendDislikeResponse(int userDislikedId)
    {
        var userId = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            await Clients.Caller.SendAsync("Error", "User not authenticated.");
            return;
        }

        var result = await _likeService.SendDislikeResponse(int.Parse(userId), userDislikedId);
        if (result.IsSuccess)
        {
            await Clients.Caller.SendAsync("DislikeResponseSend", userDislikedId);
        }
        else
        {
            await Clients.Caller.SendAsync("Error", result.Message);
        }
    }

    public async Task SendLikeResponse(int userLikedId)
    {
        var userId = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            await Clients.Caller.SendAsync("Error", "User not authenticated.");
            return;
        }

        var result = await _likeService.SendLikeResponse(int.Parse(userId), userLikedId);
        if (result.IsSuccess)
        {
            await Clients.Caller.SendAsync("LikeResponseSend", userLikedId);
        }
        else
        {
            await Clients.Caller.SendAsync("Error", result.Message);
        }
    }
}