using WebSocket.Domain.Entity;

namespace WebSocket.Domain.dto;

public class MessageDto
{
    public Guid SenderId { get; set; }
    public string Content { get; set; }
    public DateTime SentAt { get; set; }
    public bool IsRead { get; set; }
}