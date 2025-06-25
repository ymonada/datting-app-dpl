using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using WebSocket.dto;
using WebSocket.Features.User;
using WebSocket.Service;

namespace WebSocket.Features.Chats;
[Authorize]

public class ProfileHub : Hub
{
    private readonly UserService _userService;
    private readonly LikeService _likeService;

    public ProfileHub(UserService userService, LikeService likeService)
    {
        _likeService = likeService;
        _userService = userService;
    }
    private int GetUserId()
    {
        var userIdClaim = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0";

        return int.Parse(userIdClaim);
    }

    public async Task GetProfile(CancellationToken ct)
    {
        try
        {
            var userId = GetUserId();
            if (userId == 0)
            {
                throw new HubException("User is not authenticated.");
            }
            var result = await _userService.GetUserInfo(userId, ct);
        }
        catch (HubException ex)
        {
            await Console.Error.WriteLineAsync($"Error in GetProfile: {ex.Message} \"An unexpected error occurred while fetching the profile.\"");
        }
    }

    // public async Task ProfileUpdate(UserUpdateProfileDto profile, CancellationToken ct)
    // {
    //     try
    //     {
    //         var userId = GetUserId();
    //         if (userId == 0)
    //         {
    //             throw new HubException("User is not authenticated.");
    //         }
    //         var result = await _userService.UpdateMyProfile(userId, profile, ct);
    //         if (result.IsSuccess)
    //         {
    //             await Clients.Caller.SendAsync("ReceiveProfile", result.Data);
    //         }
    //         await Clients.Caller.SendAsync("ReceiveProfile", result.Message);
    //     }
    //     catch (HubException ex)
    //     {
    //         await Console.Error.WriteLineAsync($"Error in GetProfile: {ex.Message} \"An unexpected error occurred while fetching the profile.\"");
    //     }
    // }

    public async Task GetMyLikes()
    {
        try
        {
            var userId = GetUserId();
            if (userId == 0)
            {
                throw new HubException("User is not authenticated.");
            }

            var result = await _likeService.GetLikes(userId);
            if (result.IsSuccess)
            {
                
            }
        }
        catch (HubException ex)
        {
            await Console.Error.WriteLineAsync($"Error in GetProfile: {ex.Message} \"An unexpected error occurred while fetching the profile.\"");
        }
    }
}
