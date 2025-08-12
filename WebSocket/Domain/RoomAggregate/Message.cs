using WebSocket.Domain.Common;
using WebSocket.Domain.UserAggregate;

namespace WebSocket.Domain.RoomAggregate;

public class Message: Entity<Guid>
{
    public User User { get; set; }
    public Guid UserId { get; set; }
    public Room Room { get; set; }
    public Guid RoomId { get; set; }
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
