using WebSocket.Contracts.User.Profile;
using WebSocket.Domain.Common;
using WebSocket.Domain.Enums;

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
    public Profile(Guid id):base(id) { }

    public void Update(ProfileDto dto)
    {
        Name = dto.Name;
        Age = dto.Age;
        Bio = dto.Bio;
        Gender = dto.Gender;
        GenderPreference = dto.GenderPreference;
        Location = dto.Location;
    }
    public string Name { get; private set; } = string.Empty;
    public byte Age {get; private set; } =  0;
    public string Bio {get; private set; } =  string.Empty;
    public Gender Gender { get; private set; } = Gender.Any;
    public Gender GenderPreference { get; private set; } = Gender.Any;
    public Location Location {get; private set; } =  Location.CreateEmpty();
    
    public static Profile CreateEmpty(Guid id) => new Profile(id);
}
