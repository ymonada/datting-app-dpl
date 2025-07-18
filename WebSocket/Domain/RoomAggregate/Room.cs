using WebSocket.Domain.Common;

namespace WebSocket.Domain.RoomAggregate;

public class Room : Entity<Guid>
{
   
    public ICollection<UserRoom> UserRooms { get; private set; } = new List<UserRoom>();
    private Room(Guid id):base(id)
    {
        
    }

    public static Room Create(Guid id) => new Room(id);
   
}