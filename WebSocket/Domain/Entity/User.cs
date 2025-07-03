using System.Text.Json.Serialization;
using Microsoft.IdentityModel.Tokens;
using WebSocket.Domain.dto;
using WebSocket.Domain.Entity;
using WebSocket.dto;
using WebSocket.Features.User;

namespace WebSocket.Entity;

public enum Gender
{
    Male,
    Female,
    Any
}

public class User
{
    public int Id { get; set; }
    public bool IsActive { get; set; } 
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
    public Gender Gender { get; set; }
    public Gender GenderPreference { get; set; } 
    public string City { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public DateTime LastActiveTime { get; set; } = DateTime.UtcNow;
    public ICollection<UserRole> Roles { get; set; } = [];
    public ICollection<Like> ReceiveLikes { get; set; } = [];
    public ICollection<Photo> Photos { get; set; } = [];
    public ICollection<ProfileHistory> ProfileHistory { get; set; } = [];
    public ICollection<Match> Matches { get; set; } = [];
    public bool IsFullProfile ()=> Photos.Count > 0 && City.IsNullOrEmpty() && Age > 6;
    public UserDto ToDto() => new UserDto(
        Id
        , Name
        , Age
        , City
        , Bio
        , Gender
        , GenderPreference
        , Photos.Select(r => new PhotoDto(r.Id, r.Url, r.ContentType)).ToList()
    );
}