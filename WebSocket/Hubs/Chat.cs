using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using WebSocket.Entity;
using WebSocket.Service;

namespace WebSocket.Hubs;

[Authorize]
public class Chat : Hub
{
    private readonly ILogger<Chat> _logger;
    private readonly UserService _userService;

    public Chat(ILogger<Chat> logger, UserService userService)
    {
        _logger = logger;
        _userService = userService;
    }

    private int GetUserId() =>int.Parse(Context.User
        .FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
    
    public async Task SendMessage(string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", message);
        _logger.LogInformation($"Message received: {message}");
    }
    
    // public async Task GetProfile()
    // {   var userId = GetUserId();
    //     _logger.LogInformation($"\n\n\n\n\n\nGet profile: {userId}\n\n\n\n\n\n\n\n\n\n\n");
    //     var message = "No user found";
    //     if (userId == 0)
    //     {
    //         message= "No authenticated user";
    //     }
    //     var result = await _userService.GetUserInfo(userId);
    //     if (result.IsSuccess)
    //     {
    //         await Clients.Caller.SendAsync("ReceiveProfile",result.Data);
    //         _logger.LogInformation($"User {result.Data} logged in");
    //     }
    //     else
    //     {
    //         await Clients.Caller.SendAsync("ReceiveProfile", message);
    //     }
    public async Task GetProfile()
    {
        var userId = GetUserId();
        _logger.LogInformation($"📢 Extracted userId: {userId}");

        if (userId == 0)
        {
            _logger.LogWarning("⚠️ No authenticated user");
            await Clients.Caller.SendAsync("ReceiveProfile", "No authenticated user");
            return;
        }

        var result = await _userService.GetUserInfo(userId);
        if (result.IsSuccess)
        {
            _logger.LogInformation($"✅ Found user: {result.Data}");
            await Clients.Caller.SendAsync("ReceiveProfile", result.Data);
        }
        else
        {
            _logger.LogWarning("⚠️ No user found");
            await Clients.Caller.SendAsync("ReceiveProfile", "No user found");
        }
    }


       
       
           
}


   