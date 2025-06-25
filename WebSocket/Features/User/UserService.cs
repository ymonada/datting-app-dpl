
using System.Runtime.InteropServices.JavaScript;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebSocket.Errors;
using WebSocket.db;
using WebSocket.dto;
using WebSocket.Entity;
using WebSocket.Service;

namespace WebSocket.Features.User;

public class UserService 
{
    private readonly AppDbContext _context;
    public UserService(AppDbContext context)
    {
        _context = context;
    }
    private  async Task<Entity.User?> GetCurrentUser(int userId, CancellationToken ct)
    {
        return await _context.Users
            .Include(u => u.Roles)
            .ThenInclude(ur => ur.Role)
            .Include(u=>u.Photos)
            .Include(u=>u.ReceiveLikes)
            .ThenInclude(u=>u.UserFrom)
            .FirstOrDefaultAsync(u=> u.Id == userId, ct);
    }
    private async Task<bool> ProfileIsFull(int userId, CancellationToken ct)
    {
        var user = await GetCurrentUser(userId ,ct);
        if (user is { Photos: not null } 
            && !user.City.IsNullOrEmpty() 
            && user.Age > 6)
        {
            return true;
        }
        return false;
    }
    public async Task<Fin<UserDto,Error>> GetUserInfo(int idUser, CancellationToken ct)
    {
        var user = await GetCurrentUser(idUser, ct);
        if (user == null)
            return Error.NotFound();
        return user.ToDto();
    }
    public async Task<Fin<UserDto, Error>> UpdateMyProfile(int userId, UserUpdateProfileDto newProfileDto,  CancellationToken ct)
    {
        var currentUser = await GetCurrentUser(userId, ct);
        if (currentUser == null)
            return Error.NotFound("User not found");
        if(newProfileDto.Age is < 10 or > 80)
            return Error.Validation("Age must be between 10 and 80");
        await _context.Users.Where(u=>u.Id == userId)
            .ExecuteUpdateAsync(i => i
                .SetProperty(o=>o.Name, newProfileDto.Name)
                .SetProperty(o=>o.Age, newProfileDto.Age)
                .SetProperty(o=>o.City, newProfileDto.City)
                .SetProperty(o=>o.Bio, newProfileDto.Bio)
                .SetProperty(o=>o.Gender, newProfileDto.Gender)
                .SetProperty(o=>o.GenderPreference, newProfileDto.GenderPreference)
                .SetProperty(o=>o.LastActiveTime, DateTime.UtcNow), ct);
        await _context.SaveChangesAsync(ct);
        return currentUser.ToDto();
    }
    public async Task<string> DropDatabase()
    {
       await _context.Users
           .Where(u=>u.Age > 0)
           .ExecuteDeleteAsync();
        return "ok";
    }
    public async Task<Fin<string, Error>> SetProfileActive(int userId)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null)
            return Error.NotFound("User not found");
        if (user.IsActive)
            return "User is already active";
        user.IsActive = true;
        await _context.SaveChangesAsync();
        return "User is now active";
    }
    public async Task SetActiveAll()
    {
        await _context.Users.ExecuteUpdateAsync(u => u
            .SetProperty(r => r.IsActive, true));
        await _context.SaveChangesAsync();
    }

    public async Task AddToHistory(int userId, int archiveId)
    {
        var hs = await _context.ProfileHistories
            .FirstOrDefaultAsync(h => h.UserId == userId && h.WatchId == archiveId);
        if(hs == null)
        {
            await _context.ProfileHistories.AddAsync(new ProfileHistory()
            {
                UserId = userId,
                WatchId = archiveId

            });
            await _context.SaveChangesAsync();
        }
        await SetProfileActive(userId);
    }

    public async Task<Fin<List<UserDto>, Error>> FindProfileAsync(int userId)
    {
        var currentUser = await _context.Users
            .Include(u => u.Photos)
            .Include(u=>u.ReceiveLikes)
            .Include(u=>u.Roles)
            .ThenInclude(ur => ur.Role)
            .Include(u=>u.ProfileHistory)
            .FirstOrDefaultAsync(u => u.Id == userId);
  
        if (currentUser == null)
            return Error.NotFound("User not found");
        var activeUser = await GetActivityUsers(currentUser);
        if (activeUser.Count >= 1)
            return activeUser.Select(x=>x.ToDto()).ToList();
        await ClearHistory(userId);
        activeUser = await GetActivityUsers(currentUser);
        if (activeUser.Count <= 1)
            return Error.NotFound("User not found");
        return activeUser.Select(x=>x.ToDto()).ToList();
    }
    private async Task<List<Entity.User>> GetActivityUsers(Entity.User currentUser)
    {
        var viewedUserIds = currentUser.ProfileHistory
            .Select(p => p.WatchId).ToList();
        return await _context.Users
            .Where(u => u.IsActive && !viewedUserIds
                .Contains(u.Id) && u.Id != currentUser.Id)
            .Include(u => u.Photos)
            .OrderByDescending(u => u.LastActiveTime)
            .Take(5)
            .ToListAsync();
    }
    private async Task ClearHistory(int userId)
    {
        var user = await _context.ProfileHistories
            .Where(u => u.UserId == userId)
            .ExecuteDeleteAsync();
        await _context.SaveChangesAsync();
    }
}
