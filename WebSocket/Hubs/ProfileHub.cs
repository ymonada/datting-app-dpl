using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SignalRSwaggerGen.Attributes;
using WebSocket.dto;
using WebSocket.Service;

namespace WebSocket.Hubs;
[Authorize]
[SignalRHub]
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

    public async Task GetProfile()
    {
        try
        {
            var userId = GetUserId();
            if (userId == 0)
            {
                throw new HubException("User is not authenticated.");
            }
            var result = await _userService.GetUserInfo(userId);
            if (result.IsSuccess)
            {
                await Clients.Caller.SendAsync("ReceiveProfile", result.Message);
            }
            else
            {
                await Clients.Caller.SendAsync("ReceiveProfile", result.Message);
            }

        }
        catch (HubException ex)
        {
            await Console.Error.WriteLineAsync($"Error in GetProfile: {ex.Message} \"An unexpected error occurred while fetching the profile.\"");
        }
    }

    public async Task ProfileUpdate(UserUpdateProfileDto profile)
    {
        try
        {
            var userId = GetUserId();
            if (userId == 0)
            {
                throw new HubException("User is not authenticated.");
            }
            var result = await _userService.UpdateMyProfile(userId, profile);
            if (result.IsSuccess)
            {
                await Clients.Caller.SendAsync("ReceiveProfile", result.Data);
            }
            await Clients.Caller.SendAsync("ReceiveProfile", result.Message);
        }
        catch (HubException ex)
        {
            await Console.Error.WriteLineAsync($"Error in GetProfile: {ex.Message} \"An unexpected error occurred while fetching the profile.\"");
        }
    }

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
