using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using WebSocket.db;
using WebSocket.dto;
using WebSocket.Entity;
using WebSocket.Service;

namespace WebSocket.Hubs;

[Authorize]
public class ChatHub : Hub
{
    private readonly AppDbContext _context;

    public ChatHub(AppDbContext context)
    {
        _context = context;
    }

    public async Task SendMessage(string receiverId, string message)
    {
        var senderId = int.Parse(Context.UserIdentifier);
        var match = await _context.Matches
            .FirstOrDefaultAsync(m => (m.FirstUserId == senderId && m.SecondUserId == int.Parse(receiverId)) ||
                                      (m.FirstUserId == int.Parse(receiverId) && m.SecondUserId == senderId));

        if (match == null) throw new HubException("Match not found");

        var msg = new Message
        {
            MatchId = match.Id,
            SenderId = senderId,
            Content = message,
            SentAt = DateTime.UtcNow,
            IsRead = false
        };

        _context.Messages.Add(msg);
        await _context.SaveChangesAsync();

        await Clients.User(receiverId).SendAsync("ReceiveMessage", senderId.ToString(), message);
    }
}


   