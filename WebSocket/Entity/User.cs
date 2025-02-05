using System.Text.Json.Serialization;

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

    public static ReadOnlySpan<byte> FindFirstValue(string httpSchemasXmlsoapOrgWsIdentityClaimsNameidentifier)
    {
        throw new NotImplementedException();
    }
}