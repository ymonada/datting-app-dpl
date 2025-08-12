using WebSocket.Contracts;
using WebSocket.Contracts.User;
using WebSocket.Domain.Common;

namespace WebSocket.Domain.UserAggregate;

public class Like : Entity<Guid>
{
    public User User { get; private set; }
    public Guid UserId { get; set; }
    public Guid UserSenderId { get; private set; }
    public string Message { get; private set; } 
    public LikeDto AsDto()=> new LikeDto(Id, UserId, UserSenderId, Message);
    private Like(Guid id, Guid userId, Guid userSenderId, string message):base(id)
    {
        UserId = userId;
        UserSenderId = userSenderId;
        Message = message;
    }
    public static Like Create(Guid id,Guid userId, Guid userSenderId, string message = "") => new Like(id, userId, userSenderId, message);
    
}