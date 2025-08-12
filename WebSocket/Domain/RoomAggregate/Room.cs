using WebSocket.Domain.Common;
using WebSocket.Domain.UserAggregate;

namespace WebSocket.Domain.RoomAggregate;

public class Room : Entity<Guid>
{
   
    public ICollection<UserRoom> UserRooms { get; private set; }
    public ICollection<Message> Messages { get; private set; }

    public Room(Guid id) : base(id){}
    private Room(Guid id, UserRoom first, UserRoom second):base(id)
    {
        UserRooms = new List<UserRoom>() { first, second };
    }

    public static Room CreateRoomWithTwoUser(Guid id, User first, User second)
    {
        var urFirst = UserRoom.Create(id, first);
        var urSecond = UserRoom.Create(id, second);
        return new Room(Guid.NewGuid(), urFirst, urSecond);
    }
   
}