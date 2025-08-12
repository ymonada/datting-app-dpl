using WebSocket.Contracts.User;
using WebSocket.Domain.UserAggregate;

namespace WebSocket.Domain.Entity;

public class Photo
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; } 
    public string Url { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    
    
}