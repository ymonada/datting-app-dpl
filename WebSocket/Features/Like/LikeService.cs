using Microsoft.EntityFrameworkCore;
using WebSocket.db;
using WebSocket.Domain.Entity;
using WebSocket.Domain.dto;
using WebSocket.Features.User;
using WebSocket.Service;
using WebSocket.Errors;

namespace WebSocket.Features.Like;

public class LikeService
{
    private readonly AppDbContext _context;
    private readonly UserService _userService;

    public LikeService(AppDbContext context, UserService userService)
    {
        _context = context;
        _userService = userService;
    }
    // private  async Task<Domain.Entity.User?> GetCurrentUser(int userId)
    // {
    //     return await _context.UserRooms
    //         .Include(u => u.Roles)
    //         .ThenInclude(ur => ur.Role)
    //         .Include(u=>u.Photos)
    //         .Include(u=>u.ReceiveLikes)
    //         .ThenInclude(u=>u.UserFrom)
    //         .FirstOrDefaultAsync(u=> u.Id == userId);
    // }
    // public async Task<Fin<LikeDto, Error>> Like(int idSender, int idFollower)
    // {
    //     var lk = await _context.Likes.FirstOrDefaultAsync(l => 
    //         l.UserToId == idFollower && l.UserFromId == idSender);
    //     if (lk != null)
    //         return lk.ToDto();
    //     var like = new Domain.Entity.Like
    //     {
    //         UserFromId = idSender,
    //         UserToId = idFollower
    //     };
    //     await _context.Likes.AddAsync(like);
    //     await _userService.AddToHistory(idSender, idFollower);
    //     await _context.SaveChangesAsync();
    //     return like.ToDto();
    // }
    //
    // public async Task Pass(int userId, int idUserDisliked) =>
    //     await _userService.AddToHistory(userId, idUserDisliked);
    //
    // public async Task<Fin<MatchDto,Error>> LikeResponse(int userId, int idFollower)
    // {
    //     var currentUser = await GetCurrentUser(userId);
    //     var followerUser = await GetCurrentUser(idFollower);
    //     if(currentUser == null ||  followerUser == null)
    //         return Error.NotFound("Not found user and follower");
    //     var like = await _context.Likes
    //         .FirstOrDefaultAsync(l => l.UserToId == userId & l.UserFromId == idFollower);
    //     if (like == null || !currentUser.ReceiveLikes.Contains(like))
    //         return Error.NotFound("Not like found");
    //     _context.Likes.Remove(like);
    //
    //     var matchExist = await _context.Matches
    //         .FirstOrDefaultAsync(u=>
    //             (u.FirstUserId == currentUser.Id && u.SecondUserId == followerUser.Id)
    //         || (u.FirstUserId == followerUser.Id && u.SecondUserId == currentUser.Id));
    //     if (matchExist !=  null)
    //        return matchExist.ToDto();
    //     var match = new Match
    //     {
    //         FirstUserId = currentUser.Id,
    //         SecondUserId = followerUser.Id
    //     };
    //     await _context.Matches.AddAsync(match);
    //     await _context.SaveChangesAsync();
    //     return match.ToDto();
    // }
    // public async Task<Fin<LikeDto, Error>> PassResponse(int userId, int idFollower)
    // {
    //     var like = await _context.Likes
    //         .FirstOrDefaultAsync(l => l.UserToId == userId & l.UserFromId == idFollower);
    //     if (like == null)
    //         return Error.NotFound($"Not like found userFrm {userId} userTo {idFollower}");
    //     _context.Likes.Remove(like);
    //     await _context.SaveChangesAsync();
    //     return like.ToDto();
    // }
    // public async Task<Fin<List<UserDto>,Error>> GetLikes(int userId)
    // {
    //     if (await GetCurrentUser(userId) == null)
    //         return Error.NotFound("Not found user");
    //     var users = await _context.Likes
    //         .Where(u => u.UserToId == userId)
    //         .Select(u => u.UserFrom.ToDto())
    //         .ToListAsync();
    //     return users;
    // }
}
