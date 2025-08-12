using System.ComponentModel.DataAnnotations;
using System.Runtime.ExceptionServices;
using ErrorOr;
using FluentValidation;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using WebSocket.Contracts;
using WebSocket.Contracts.Extensions;
using WebSocket.Contracts.User;
using WebSocket.db;
using WebSocket.Domain;
using WebSocket.Domain.Entity;
using WebSocket.Domain.Enums;
using WebSocket.Domain.RoomAggregate;
using WebSocket.Domain.UserAggregate;
using WebSocket.Domain.Validators;
using WebSocket.Domain.ValueObjects;
using WebSocket.Features.Photos;

namespace WebSocket.Features.User;

public class UserService
{
    private readonly AppDbContext _context;
    private readonly IValidator<Email> _emailValidator;
    private readonly IValidator<Credentials> _credentialsValidator;
    private readonly IValidator<Location> _locationValidator;
    private readonly PhotoService _photoService;

    public UserService(
        AppDbContext context,
        PhotoService photoService,
        IValidator<Email> emailValidator,
        IValidator<Credentials> credentialsValidator,
        IValidator<Location> locationValidator
    )
    {
        _context = context;
        _photoService = photoService;
        _emailValidator = emailValidator;
        _credentialsValidator = credentialsValidator;
        _locationValidator = locationValidator;
    }

    private async Task<Domain.UserAggregate.User?> GetCurrentUser(Guid userId, CancellationToken ct)
    {
        return await _context.Users
            .Where(c => c.Id == userId)
            .Include(c => c.Profile)
            .Include(u=>u.ReceiveLikes)
            .Include(c => c.Photos)
            .Include(v => v.Rooms)
                .ThenInclude(x=>x.Room)
            .FirstOrDefaultAsync(ct);
    }

    public async Task<ErrorOr<UserDto>> User(Guid userId, CancellationToken ct)
    {
        var user = await GetCurrentUser(userId, ct);
        return user is not null ? user.AsDto() : Error.Unauthorized(description: "User not found"); 
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

    public record SendLikeRequestDto(Guid ReceiverId, string Message);
    
    
    //list
    public async Task<ErrorOr<Success>> SendLikeToUsers(Guid senderId, List<SendLikeRequestDto> likes, CancellationToken ct)
    {
        var sender = await GetCurrentUser(senderId, ct);
        if (sender is null) return Error.NotFound(description:$"User {senderId} not found or unactive");
        var receiverIds = likes.Select(x => x.ReceiverId).ToHashSet();
        var users = await _context.Users
            .Where(c => likes
                .Select(x => x.ReceiverId)
                .Contains(c.Id))
            .Include(c => c.ReceiveLikes)
            .ToHashSetAsync(ct);
        var lk = likes.ToDictionary(x => x.ReceiverId, x=>x.Message);
        foreach (var u in users)
        {
            if(u.ReceiveLikes
               .Select(x=>x.UserSenderId)
               .ToHashSet()
               .Contains(senderId)) continue;
            
            var newLike = Domain.UserAggregate.Like.Create(Guid.NewGuid(),u.Id, senderId, lk[u.Id]);
            u.SendLike(newLike);
            await _context.Likes.AddAsync(newLike, ct);
        }
        await _context.SaveChangesAsync(ct);
        return default;
    }

    public record LikeResponseDto(Guid LikeId, Guid SenderId, bool IsResponses);
    //uno
   
    public async Task<ErrorOr<Success>> TakeLikeResponses(Guid userId, LikeResponseDto like, CancellationToken ct)
    {
        var user = await GetCurrentUser(userId, ct);
        if(user is null) return Error.NotFound(description: "User not found");
        
        var sender = await GetCurrentUser(userId, ct);
        if(sender is null) return Error.NotFound(description: "User not found");
        
        var userLikeId = user.ReceiveLikes.Select(x => x.Id).ToHashSet();
        var userRoomsId = user.Rooms.Select(x => x.Id).ToHashSet();
       
        if (!like.IsResponses || !userLikeId.Contains(like.LikeId))
            return Error.Failure(description: "Like not found");
        var senderRoomsId = sender.Rooms.Select(x => x.Id).ToHashSet();
        var roomExist = userRoomsId.Overlaps(senderRoomsId);
        if (roomExist) 
            return default;
        var newRoom = Room.CreateRoomWithTwoUser(Guid.NewGuid(), user, sender);
        await _context.Rooms.AddAsync(newRoom, ct);
        await _context.SaveChangesAsync(ct);
        return default;
    }
   
    public async Task<ErrorOr<UserDto>> UpdateProfile(Guid userId, UserUpdateRequestDto data, CancellationToken ct)
    {
        var user = await GetCurrentUser(userId, ct);
        if (user is null) return Error.Failure(description: "User not found");
        
        var photoExists = data.Photos.Count != 0;
        List<Photo> newPhotos = [];
        if (photoExists)
        {
             var res = await _photoService.SavePhotoInMemoryAsync(userId, data.Photos);
             if(res.IsError) return res.Errors;
             _photoService.DeletePhotoFromMemoryAsync(user.Photos);
             newPhotos = res.Value;
        }
        
        user.Update(data.Profile, data.Tags, photoExists ? newPhotos : user.Photos);
        await _context.SaveChangesAsync(ct);
        return user.AsDto();
    }
}
