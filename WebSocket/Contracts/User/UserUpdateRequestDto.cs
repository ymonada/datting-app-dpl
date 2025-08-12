using WebSocket.Contracts.User.Profile;
using WebSocket.Domain.Entity;
using WebSocket.Domain.Enums;
using WebSocket.Domain.UserAggregate;


namespace WebSocket.Contracts.User;

public record UserUpdateRequestDto
{
    public ProfileDto Profile { get; init; }
    public ICollection<Tags> Tags { get; init; }
    public ICollection<IFormFile> Photos { get; init; }
}