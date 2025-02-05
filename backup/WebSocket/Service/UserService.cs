using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebSocket.db;
using WebSocket.dto;
using WebSocket.Entity;

namespace WebSocket.Service;

public class UserService 
{
    private readonly AppDbContext _context;
    public UserService(AppDbContext context)
    {
        _context = context;
    }
    private  async Task<User?> GetCurrentUser(int userId)
    {
        return await _context.Users
            .Include(u => u.Roles)
            .ThenInclude(ur => ur.Role)
            .Include(u=>u.Photos)
            .Include(u=>u.ReceiveLikes)
            .ThenInclude(u=>u.UserFrom)
            .FirstOrDefaultAsync(u=> u.Id == userId);
    }
    private  UserProfileDto GetUserDto(User user)
    {
        return new UserProfileDto(
            user.Id
            //, user.IsActive
            , user.Name
            , user.Age
            , user.City
            , user.Bio
            , user.Gender
            , user.GenderPreference
            //, user.Roles.Select(r => new RoleDto(r.RoleId, r.Role.Name)).ToList()
            , user.Photos.Select(r => new PhotoDto(r.Id, r.Url, r.ContentType)).ToList()
        );
            
    }
    private async Task<bool> ProfileIsFull(int userId)
    {
        var user = await GetCurrentUser(userId);
        if (user is { Photos: not null } 
            && !user.City.IsNullOrEmpty() 
            && user.Age > 6)
        {
            return true;
        }
        return false;
    }
    public async Task<ServiceResult<UserProfileDto>> GetUserInfo(int idUser)
    {
        var currentUser = await GetCurrentUser(idUser);
        if (currentUser == null)
        {
            return new ServiceResult<UserProfileDto>()
            {
                IsSuccess = false, Message = "No user found"
            };
        }
        var currentUserDto = GetUserDto(currentUser);
        return new ServiceResult<UserProfileDto>()
        {
            IsSuccess = true
            , Message = "ok"
            , Data =  new UserProfileDto(
                currentUser.Id
                //, currentUser.IsActive
                , currentUser.Name
                , currentUser.Age
                , currentUser.City
                , currentUser.Bio
                , currentUser.Gender
                , currentUser.GenderPreference
                //, currentUser.Roles.Select(r => new RoleDto(r.RoleId, r.Role.Name)).ToList()
                , currentUser.Photos.Select(r => new PhotoDto(r.Id, r.Url, r.ContentType)).ToList()
                )
        };
    }
    public async Task<ServiceResult<UserProfileDto>> UpdateMyProfile(int userId, [FromBody] UserUpdateProfileDto newProfileDto)
    {
        var currentUser = await GetCurrentUser(userId);
        var badResult = new ServiceResult<UserProfileDto>
        {
            IsSuccess = false
        };
        if (currentUser == null)
        {
            badResult.Message = "User not found";
            return badResult;
        }
        if(newProfileDto.Age is < 10 or > 80)
        {
            badResult.Message = "Age is not valid";
            return badResult;
        }
        await _context.Users.Where(u=>u.Id == userId)
            .ExecuteUpdateAsync(i => i
                .SetProperty(o=>o.Name, newProfileDto.Name)
                .SetProperty(o=>o.Age, newProfileDto.Age)
                .SetProperty(o=>o.City, newProfileDto.City)
                .SetProperty(o=>o.Bio, newProfileDto.Bio)
                .SetProperty(o=>o.Gender, newProfileDto.Gender)
                .SetProperty(o=>o.GenderPreference, newProfileDto.GenderPreference)
                .SetProperty(o=>o.LastActiveTime, DateTime.UtcNow));
        await _context.SaveChangesAsync();
        return new ServiceResult<UserProfileDto>
        {
            IsSuccess = true, Message = "User updated"
        };
    }
    public async Task<string> DropDatabase()
    {
       await _context.Users
           .Where(u=>u.Age > 0)
           .ExecuteDeleteAsync();
        return "ok";
    }
    public async Task<ServiceResult<string>> SetProfileActive(int userId)
    {
        // if (!await ProfileIsFull(userId))
        //     return new ServiceResult<string>
        //     {
        //         IsSuccess = false, Message = "Profile not full"
        //     };
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null)
            return new ServiceResult<string>
            {
                IsSuccess = false, Message = "Profile not found"
            };
        if (user.IsActive)
            return new ServiceResult<string>
            {
                IsSuccess = true, Message = "Profile is active"
            };
        user.IsActive = true;
        await _context.SaveChangesAsync();
        return new ServiceResult<string>
        {
            IsSuccess = true, Message = "Profile is active"
        };
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

    public async Task<ServiceResult<List<UserProfileDto>>> Finding(int userId)
    {
        var currentUser = await _context.Users
            .Include(u => u.Photos)
            .Include(u=>u.ReceiveLikes)
            .Include(u=>u.Roles)
            .ThenInclude(ur => ur.Role)
            .Include(u=>u.ProfileHistory)
            .FirstOrDefaultAsync(u => u.Id == userId);
        if (currentUser == null)
        {
            return new ServiceResult<List<UserProfileDto>>
            {
                IsSuccess = false, Message = "User not found"
            };
        }
        //var viewedUserIds = currentUser.ProfileHistory.Select(p => p.WatchId).ToList();
        var activeUser = await GetActivityUsers(currentUser);

        if (activeUser.Count >= 1)
            return Responce();
        await ClearHistory(userId);
        activeUser = await GetActivityUsers(currentUser);
        if (activeUser.Count <= 1)
        {
            return new ServiceResult<List<UserProfileDto>>
            {
                IsSuccess = false, Message = "Users not found"
            };
        }
        return Responce();

        ServiceResult<List<UserProfileDto>> Responce()
        {
            return new ServiceResult<List<UserProfileDto>>
            {
                IsSuccess = true,
                Message = "Users found",
                Data = activeUser.Select(u => new UserProfileDto(
                    u.Id
                    //, u.IsActive
                    , u.Name
                    , u.Age
                    , u.City
                    , u.Bio
                    , u.Gender
                    ,u.GenderPreference
                    //, u.Roles.Select(r => new RoleDto(r.RoleId, r.Role.Name)).ToList()
                    , u.Photos.Select(r => new PhotoDto(r.Id, r.Url, r.ContentType)).ToList())).ToList()
            };
        }
    }
    private async Task<List<User>> GetActivityUsers(User currentUser)
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
