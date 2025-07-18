using WebSocket.Domain.UserAggregate;

namespace WebSocket.Domain.Entity;

public readonly record struct PhotoId
{
    public readonly Guid Value;
    public PhotoId(Guid value) => Value = value;
    public static PhotoId New() => new(Guid.NewGuid());
    public static PhotoId Empty() => new(Guid.Empty);
};
public class Photo
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; } 
    public string Url { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
}