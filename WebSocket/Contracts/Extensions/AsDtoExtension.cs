using WebSocket.Contracts.User;
using WebSocket.Contracts.User.Profile;
using WebSocket.Domain.Entity;
using WebSocket.Domain.UserAggregate;
using WebSocket.Features.Photos;

namespace WebSocket.Contracts.Extensions;

public static class AsDtoExtension
{
    public static UserDto AsDto(this Domain.UserAggregate.User user) => new UserDto(
        user.Id
        , user.Profile.AsDto()
        , user.Photos.Select(r => new PhotoDto(r.Id, r.Url, r.ContentType)).ToList()
        , user.ReceiveLikes.Select(r=> new LikeDto(r.Id, r.UserId, r.UserSenderId, r.Message)).ToList()
        , user.Rooms.Select(r=> new UserRoomDto(r.RoomId, r.UserId)).ToList()
    );

    public static PhotoDto AsDto(this Photo photo) => new PhotoDto(
        photo.Id
        , photo.Url
        , photo.ContentType);
    public static ProfileDto AsDto(this Profile profile) => new ProfileDto(
        profile.Name
        , profile.Age
        , profile.Bio
        , profile.Gender
        , profile.GenderPreference
        , profile.Location);
}