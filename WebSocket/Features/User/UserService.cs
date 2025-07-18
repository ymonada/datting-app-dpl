using System.ComponentModel.DataAnnotations;
using ErrorOr;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using WebSocket.Contracts;
using WebSocket.db;
using WebSocket.Domain;
using WebSocket.Domain.Entity;
using WebSocket.Domain.UserAggregate;
using WebSocket.Domain.Validators;
using WebSocket.Domain.ValueObjects;

namespace WebSocket.Features.User;

public class UserService
{
    private readonly AppDbContext _context;
    private readonly IValidator<Email> _emailValidator;
    private readonly IValidator<Credentials> _credentialsValidator;
    private readonly IValidator<Location> _locationValidator;

    public UserService(
        AppDbContext context,
        IValidator<Email> emailValidator,
        IValidator<Credentials> credentialsValidator,
        IValidator<Location> locationValidator
    )
    {
        _context = context;
        _emailValidator = emailValidator;
        _credentialsValidator = credentialsValidator;
        _locationValidator = locationValidator;
    }

    private async Task<Domain.UserAggregate.User?> GetCurrentUser(Guid userId, CancellationToken ct)
    {
        return await _context
            .Users.Where(c => c.Id == userId)
            .Include(c => c.Profile)
            .Include(u=>u.ReceiveLikes)
            .Include(c => c.Photos)
            .Include(v => v.Rooms)
            .FirstOrDefaultAsync(ct);
    }

    public async Task<ErrorOr<UserDto>> RegisterUser(
        Email email,
        Credentials credentials,
        CancellationToken ct
    )
    {
        var errors = new List<ValidationResult>();
        var emailResult = await _emailValidator.ValidateAsync(email, ct);

        var credentialResult = await _credentialsValidator.ValidateAsync(credentials, ct);
        if (!emailResult.IsValid || !credentialResult.IsValid)
            return Error.Validation(description: emailResult.Errors.First().ErrorMessage);

        var user = Domain.UserAggregate.User.CreateEmpty(email, credentials);
        await _context.Users.AddAsync(user, ct);
        await _context.SaveChangesAsync(ct);
        return user.AsDto();
    }

    public record LikeRequest(Guid ReceiveId, Guid SenderId, string Message);

    public async Task<ErrorOr<LikeDto>> SendLike(LikeRequest like, CancellationToken ct)
    {
        var userReceive = await GetCurrentUser(like.ReceiveId, ct);
        if (userReceive is null) return Error.NotFound(description:$"User {like.ReceiveId} not found or unactive");
        var newLike = Domain.UserAggregate.Like.Create(Guid.NewGuid(),userReceive.Id, like.SenderId, like.Message);
        userReceive.SendLike(newLike);
        await _context.Likes.AddAsync(newLike, ct);
        await _context.SaveChangesAsync(ct);
        return newLike.AsDto();
    }
    // public async Task<ErrorOr<UserDto> UpdateMyProfile(int userId, UserUpdateProfileDto newProfileDto,  CancellationToken ct)
    // {
    //     var currentUser = await GetCurrentUser(userId, ct);
    //     if (currentUser == null)
    //         return Error.NotFound("User not found");
    //     if(newProfileDto.Age is < 10 or > 80)
    //         return Error.Validation("Age must be between 10 and 80");
    //     await _context.UserRooms.Where(u=>u.Id == userId)
    //         .ExecuteUpdateAsync(i => i
    //             .SetProperty(o=>o.Name, newProfileDto.Name)
    //             .SetProperty(o=>o.Age, newProfileDto.Age)
    //             .SetProperty(o=>o.City, newProfileDto.City)
    //             .SetProperty(o=>o.Bio, newProfileDto.Bio)
    //             .SetProperty(o=>o.Sex, newProfileDto.Gender)
    //             .SetProperty(o=>o.SexPreference, newProfileDto.GenderPreference)
    //             .SetProperty(o=>o.LastActiveTime, DateTime.UtcNow), ct);
    //     await _context.SaveChangesAsync(ct);
    //     return currentUser.ToDto();
    // }
    // public async Task<string> DropDatabase()
    // {
    //    await _context.UserRooms
    //        .Where(u=>u.Age > 0)
    //        .ExecuteDeleteAsync();
    //     return "ok";
    // }
    //
    // private async Task<Fin<string, Error>> SetProfileActive(int userId)
    // {
    //     var user = await _context.UserRooms.FirstOrDefaultAsync(u => u.Id == userId);
    //     if (user == null)
    //         return Error.NotFound("User not found");
    //     if (user.IsActive)
    //         return "User is already active";
    //     user.IsActive = true;
    //     await _context.SaveChangesAsync();
    //     return "User is now active";
    // }
    // public async Task SetActiveAll()
    // {
    //     await _context.UserRooms.ExecuteUpdateAsync(u => u
    //         .SetProperty(r => r.IsActive, true));
    //     await _context.SaveChangesAsync();
    // }
    //
    // public async Task AddToHistory(int userId, int archiveId)
    // {
    //     var hs = await _context.ProfileHistories
    //         .FirstOrDefaultAsync(h => h.UserId == userId && h.WatchId == archiveId);
    //     if(hs == null)
    //     {
    //         await _context.ProfileHistories.AddAsync(new ProfileHistory()
    //         {
    //             UserId = userId,
    //             WatchId = archiveId
    //
    //         });
    //         await _context.SaveChangesAsync();
    //     }
    //     await SetProfileActive(userId);
    // }
    //
    // public async Task<Fin<List<UserDto>, Error>> FindProfileAsync(int userId)
    // {
    //     var currentUser = await _context.UserRooms
    //         .Include(u => u.Photos)
    //         .Include(u=>u.ReceiveLikes)
    //         .Include(u=>u.Roles)
    //         .ThenInclude(ur => ur.Role)
    //         .Include(u=>u.ProfileHistory)
    //         .FirstOrDefaultAsync(u => u.Id == userId);
    //
    //     if (currentUser == null)
    //         return Error.NotFound("User not found");
    //     var activeUser = await GetActivityUsers(currentUser);
    //     if (activeUser.Count >= 1)
    //         return activeUser.Select(x=>x.ToDto()).ToList();
    //     await ClearHistory(userId);
    //     activeUser = await GetActivityUsers(currentUser);
    //     if (activeUser.Count <= 1)
    //         return Error.NotFound("User not found");
    //     return activeUser.Select(x=>x.ToDto()).ToList();
    // }
    // private async Task<List<Domain.Entity.User>> GetActivityUsers(Domain.Entity.User currentUser)
    // {
    //     var viewedUserIds = currentUser.ProfileHistory
    //         .Select(p => p.WatchId).ToList();
    //     return await _context.UserRooms
    //         .Where(u => u.IsActive && !viewedUserIds
    //             .Contains(u.Id) && u.Id != currentUser.Id)
    //         .Include(u => u.Photos)
    //         .OrderByDescending(u => u.LastActiveTime)
    //         .Take(5)
    //         .ToListAsync();
    // }
    // private async Task ClearHistory(int userId)
    // {
    //     var user = await _context.ProfileHistories
    //         .Where(u => u.UserId == userId)
    //         .ExecuteDeleteAsync();
    //     await _context.SaveChangesAsync();
    // }
}
