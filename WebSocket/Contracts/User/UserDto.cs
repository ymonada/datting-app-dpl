using WebSocket.Contracts.User.Profile;
using WebSocket.Domain.UserAggregate;
using WebSocket.Features.Photos;

namespace WebSocket.Contracts.User;

public record UserDto(Guid Id, ProfileDto Profile, ICollection<PhotoDto> Photos, ICollection<LikeDto> ReceiveLikes, ICollection<UserRoomDto> UserRooms);