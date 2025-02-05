using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebSocket.db;
using WebSocket.dto;

namespace WebSocket.Controlles;

[Route("/Match")]
[ApiController]
public class MatchController : ControllerBase
{
    private readonly AppDbContext _context;

    public MatchController(AppDbContext context)
    {
        _context = context;
    }
    [HttpGet("/dialogs")]
    [Authorize]
    public async Task<IActionResult> Dialogs()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var result = await _context.Matches
            .Where(u => u.FirstUserId == userId || u.SecondUserId == userId)
            .Select(u=> userId == u.FirstUserId ? u.SecondUserId : u.FirstUserId)
            .ToListAsync();
        if (result == null)
            return NotFound();
        var usersDialog = await _context.Users
            .Include(u=>u.Photos)
            .Where(u=> result.Contains(u.Id))
            .Select(u => new UserProfileDto(
                u.Id
                , u.Name
                , u.Age
                , u.City
                , u.Bio
                , u.Gender
                , u.GenderPreference
                ,  u.Photos.Select(r => new PhotoDto(r.Id, r.Url, r.ContentType)).ToList())
            )
            .ToListAsync();
        return Ok(usersDialog);
    }
   
}