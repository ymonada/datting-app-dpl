using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using WebSocket.db;
using WebSocket.dto;
using WebSocket.Entity;
namespace WebSocket.Service;

public class LikeService
{
    private readonly AppDbContext _context;
    private readonly UserService _userService;

    public LikeService(AppDbContext context, UserService userService)
    {
        _context = context;
        _userService = userService;
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
    private UserProfileDto GetUserDto(User user)
    {
        return new UserProfileDto(
            user.Id
            //, user.IsActive
            , user.Name
            , user.Age
            , user.City
            , user.Bio
            ,user.Gender
            ,user.GenderPreference
            //, user.Roles.Select(r => new RoleDto(r.RoleId, r.Role.Name)).ToList()
            , user.Photos.Select(r => new PhotoDto(r.Id, r.Url, r.ContentType)).ToList()
        );
            
    }
    public async Task<ServiceResult<LikeDto>> SendLike(int idSender, int idFollower)
    {
        var lk = await _context.Likes.FirstOrDefaultAsync(l => 
            l.UserToId == idFollower && l.UserFromId == idSender);
        if (lk != null)
            return new ServiceResult<LikeDto>
            {
                IsSuccess = true, Data = new LikeDto(
                    lk.Id
                    , lk.UserFromId
                    , lk.UserToId
                    ), Message = "like already exists"
            };
        var like = new Like
        {
            UserFromId = idSender,
            UserToId = idFollower
        };
        await _context.Likes.AddAsync(like);
        await _userService.AddToHistory(idSender, idFollower);
        await _context.SaveChangesAsync();
        return new ServiceResult<LikeDto>
        {
            IsSuccess = true,
            Data = new LikeDto(like.Id, like.UserFromId, like.UserToId),
            Message = "ok"
        };
    }
    //
    public async Task SendDislike(int userId, int idUserDisliked)
    {
        await _userService.AddToHistory(userId, idUserDisliked);
        //await _context.Likes.Where(u=>u.UserToId == userId && u.UserFromId == idUserDisliked).ExecuteUpdateAsync()
    }

    public async Task<ServiceResult<MatchDto>> SendLikeResponse(int userId, int idFollower)
    {
        var currentUser = await GetCurrentUser(userId);
        var followerUser = await GetCurrentUser(idFollower);
        var like = await _context.Likes
            .FirstOrDefaultAsync(l => l.UserToId == userId & l.UserFromId == idFollower);
        if (currentUser == null || followerUser == null || like == null || !currentUser.ReceiveLikes.Contains(like))
            return new ServiceResult<MatchDto>
            {
                IsSuccess = false,
                Message = "No user or like found"
            };
        _context.Likes.Remove(like);
        ///перевіряти чи існує пара, ящо та нічого не робити
        //
        var matchExist = await _context.Matches
            .FirstOrDefaultAsync(u=>
                (u.FirstUserId == currentUser.Id && u.SecondUserId == followerUser.Id)
            || (u.FirstUserId == followerUser.Id && u.SecondUserId == currentUser.Id));
        if (matchExist ==  null)
        {
            var match = new Match
            {
                FirstUserId = currentUser.Id,
                SecondUserId = followerUser.Id
            };
            await _context.Matches.AddAsync(match);
        }
        await _context.SaveChangesAsync();
        return new ServiceResult<MatchDto>
        {
            IsSuccess = true,
            Data = new MatchDto(
                currentUser.Id,
                followerUser.Id),
            Message = "ok"
        };
    }
    public async Task<ServiceResult<LikeDto>> SendDislikeResponse(int userId, int idFollower)
    {
        var like = await _context.Likes
            .FirstOrDefaultAsync(l => l.UserToId == userId & l.UserFromId == idFollower);
        if (like == null)
        {
            return new ServiceResult<LikeDto>
            {
                IsSuccess = false,
                Message = $"Not like found userFrm {userId} userTo {idFollower}"
            };
        }
        _context.Likes.Remove(like);
        await _context.SaveChangesAsync();
        return new ServiceResult<LikeDto>
        {
            IsSuccess = true,
            Message = "ok"
        };
    }
    public async Task<ServiceResult<List<UserProfileDto>>> GetLikes(int userId)
    {
        var user = await GetCurrentUser(userId);
        if (user == null)
        {
            return new ServiceResult<List<UserProfileDto>>
            {
                IsSuccess = false,
                Message = "user not found"
            };
        }
        var users = await _context.Likes
            .Where(u => u.UserToId == userId)
            .Select(u => new UserProfileDto(
                    u.UserFrom.Id
                    //, user.IsActive
                    , u.UserFrom.Name
                    , u.UserFrom.Age
                    , u.UserFrom.City
                    , u.UserFrom.Bio
                    , u.UserFrom.Gender
                    , u.UserFrom.GenderPreference
                    //, user.Roles.Select(r => new RoleDto(r.RoleId, r.Role.Name)).ToList()
                    ,  u.UserFrom.Photos.Select(r => new PhotoDto(r.Id, r.Url, r.ContentType)).ToList())
            )
            .ToListAsync();
        return new ServiceResult<List<UserProfileDto>>
        {
            IsSuccess = true,
            Data = users,
            Message = "ok"
        };
    }
    
}
