using WebSocket.dto;
using WebSocket.Entity;

namespace WebSocket.Features.User;

public record UserDto(int Id
    , string Name
    , int Age
    , string City
    , string Bio
    , Gender Gender
    , Gender GenderPreference
    , ICollection<PhotoDto> Photos);