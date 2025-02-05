using WebSocket.Entity;
using WebSocket.Service;
using WebSocket.dto;
namespace WebSocket.dto;

public record UserProfileDto(int Id
   // , bool IsActive
    , string Name
    , int Age
    , string City
    , string Bio
    , Gender Gender
    , Gender GenderPreference
    //, ICollection<RoleDto> Roles
    , ICollection<PhotoDto> Photos);