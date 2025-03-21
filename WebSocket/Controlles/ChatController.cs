using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebSocket.db;
using WebSocket.dto;

namespace WebSocket.Controlles;

[Route("/Match")]
[ApiController]
public class ChatController : ControllerBase
{
    private readonly AppDbContext _context;

    public ChatController(AppDbContext context)
    {
        _context = context;
    }
    [HttpGet("/dialogs")]
    [Authorize]
    public async Task<IActionResult> Dialogs()
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdString))
            return Unauthorized("User ID not found in claims");

        var userId = int.Parse(userIdString);
        var result = await _context.Matches
            .Where(u => u.FirstUserId == userId || u.SecondUserId == userId)
            .Select(u => userId == u.FirstUserId ? u.SecondUserId : u.FirstUserId)
            .ToListAsync();

        if (result.Count == 0)
            return Ok(new List<UserProfileDto>()); // Повертаємо порожній список, якщо немає матчів

        var usersDialog = await _context.Users
            .Include(u => u.Photos)
            .Where(u => result.Contains(u.Id))
            .Select(u => new UserProfileDto(
                u.Id,
                u.Name,
                u.Age,
                u.City,
                u.Bio,
                u.Gender,
                u.GenderPreference,
                u.Photos.Select(r => new PhotoDto(r.Id, r.Url, r.ContentType)).ToList())
            )
            .ToListAsync();

        return Ok(usersDialog);
    }
    [HttpGet("/dialogs/{partnerId}/history")]
    [Authorize]
    public async Task<IActionResult> GetChatHistory(int partnerId)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException("User ID not found"));
    
        // Шукаємо матч між поточним користувачем і partnerId
        var match = await _context.Matches
            .FirstOrDefaultAsync(m => (m.FirstUserId == userId && m.SecondUserId == partnerId) ||
                                      (m.FirstUserId == partnerId && m.SecondUserId == userId));

        if (match == null)
            return NotFound("Match not found");

        // Отримуємо історію повідомлень
        var messages = await _context.Messages
            .Where(m => m.MatchId == match.Id)
            .OrderBy(m => m.SentAt)
            .Select(m => new MessageDto
            {
                SenderId = m.SenderId,
                Content = m.Content,
                SentAt = m.SentAt,
                IsRead = m.IsRead
            })
            .ToListAsync();

        return Ok(messages);
    }
}
