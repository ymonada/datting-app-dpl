using WebSocket.Domain.Enums;
using WebSocket.Domain.UserAggregate;

namespace WebSocket.Contracts.User.Profile;

public record ProfileDto(string Name,  byte Age, string Bio,Gender Gender, Gender GenderPreference, Location Location);