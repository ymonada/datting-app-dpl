using WebSocket.Domain.Common;
using WebSocket.Domain.UserAggregate;

namespace WebSocket.Domain.RoomAggregate;

public class Message: Entity<Guid>
{
    public UserRoom UserRoom { get; private set; }
    public Guid UserRoomId { get; private set; }
    public MessageContent Content { get; private set; }
    public Message(Guid id, MessageContent content):base(id)
    {
       Content = content;
    }

    public Message():base(Guid.NewGuid()) { }
    public static Message Create(Guid id, MessageContent content) => new Message(id, content);
}

public class MessageContent : ValueObject
{
    public string Content { get; init; }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Content;
    }
    public MessageContent(string content)
    {
        Content = content;
    }

    public static MessageContent Create(string content) => new MessageContent(content);
}
