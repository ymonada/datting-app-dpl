using WebSocket.Domain.Common;
using WebSocket.Domain.UserAggregate;

namespace WebSocket.Domain.RoomAggregate;

public class UserRoom : Entity<Guid>
{
    public UserRoom(Guid id) : base(id)
    {
        
    }

    public User User { get; set; }
    public Guid UserId { get; set; }
    public Room Room { get; set; }
    public Guid RoomId { get; set; }
    public ICollection<Message> Messages { get; private set; } 
    
}