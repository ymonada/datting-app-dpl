using WebSocket.Domain.Common;
using WebSocket.Domain.UserAggregate;

namespace WebSocket.Domain.RoomAggregate;

public class UserRoom : Entity<Guid>
{
    public User User { get; private set; }
    public Guid UserId { get; private set; }
    public Room Room { get; private set; }
    public Guid RoomId { get; private set; }

    public UserRoom() : base(Guid.NewGuid()){}
    public UserRoom(Guid id, User user) : base(id)
    {
        User = user;
        // Room = room;
    }
    
    public static UserRoom Create(Guid id, User user)
    {
        return new UserRoom(id,  user);
    }
    
}