using WebSocket.Contracts;
using WebSocket.Contracts.User;
using WebSocket.Domain.Common;
using WebSocket.Domain.dto;
using WebSocket.Domain.Entity;
using WebSocket.Domain.Enums;
using WebSocket.Domain.RoomAggregate;
using WebSocket.Domain.ValueObjects;
using ErrorOr;
using WebSocket.Contracts.User.Profile;
using WebSocket.Features.Like;
using WebSocket.Features.Photos;

namespace WebSocket.Domain.UserAggregate;

public class User : Entity<Guid>
{
    public Profile Profile { get; private set; }
    public Credentials Credentials { get; private set; }
    public Email Email { get; private set; }
    public AccountStatus AccountStatus { get; private set; }
    public DateTime LastActiveDateTime { get; private set; } = DateTime.UtcNow; 
    public DateTime CreatedDateTime { get; init; } = DateTime.UtcNow;
    public ICollection<Like> ReceiveLikes { get; private set; } = [];
    public ICollection<Tags> Tags { get; private set; } = [];
    public ICollection<Photo> Photos { get; private set; } 
    public ICollection<UserRoom> Rooms { get; private set; } = [];
    public ICollection<Message> Messages { get; private set; } = []; 
    private User(Email email, Credentials credentials)
        :base(Guid.NewGuid())
    {
        Email = email;
        Credentials = Credentials.Create(credentials.PasswordHash);
        Profile = Profile.CreateEmpty(Guid.NewGuid());
    }

    public User():base(Guid.NewGuid()) { }
    public User AddProfile(Profile profile)
    {
        Profile = profile;
        return this;
    }

    public User AddPhoto(ICollection<Photo> photos)
    {
        Photos = photos;
        return this;
    }   
    
    public void SetStatusActive() => AccountStatus = AccountStatus.FullAndActiveWeek;
    public void SetStatusInactive() => AccountStatus = AccountStatus.FullAndInactiveWeek;

    public void Update(ProfileDto profile, ICollection<Tags> tags, ICollection<Photo> photos)
    {
        Profile.Update(profile);
        Tags = tags;
        Photos = photos;
    }
    
    public static User CreateEmpty(Email email, Credentials credentials)
    {
        return new User(email, credentials);
    }

    public void SendLike(Like  like)
    {
        ReceiveLikes.Add(like);
    }
    
}
