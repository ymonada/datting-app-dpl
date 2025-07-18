using WebSocket.Domain.Common;
using WebSocket.Domain.Enums;
using WebSocket.Domain.ValueObjects;
using EnvironmentName = Microsoft.AspNetCore.Hosting.EnvironmentName;

namespace WebSocket.Domain.UserAggregate;

public class Profile : Entity<Guid>
{
    public User User { get; private set; }
    public Guid UserId { get; set; }
  
    public Profile(Guid id, string name, byte age, string bio, Gender gender, Gender genderPreference, Location location) : base(id)
    {
        Name = name;
        Age = age;
        Bio = bio;
        Gender = gender;
        GenderPreference = genderPreference;
        Location = location;
    }
    public Profile(Guid id):base(id)
    {
        
    }
    public string Name { get; init; } = string.Empty;
    public byte Age {get; init; } =  0;
    public string Bio {get; init; } =  string.Empty;
    public Gender Gender { get; init; } = Gender.Any;
    public Gender GenderPreference { get; init; } = Gender.Any;
    public Location Location {get; init; } =  Location.CreateEmpty();
    public ProfileDto AsDto() => new ProfileDto(Name, Age, Bio, Gender, GenderPreference, Location);
    public static Profile CreateEmpty(Guid id) => new Profile(id);
}
public record ProfileDto(string Name,  byte Age, string Bio,Gender Gender, Gender GenderPreference, Location Location);