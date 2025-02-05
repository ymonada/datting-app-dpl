using WebSocket.Entity;

namespace WebSocket.dto;

public record UserUpdateProfileDto(string Name
    , int Age
    , string City
    , string Bio
    , Gender Gender
    , Gender GenderPreference);