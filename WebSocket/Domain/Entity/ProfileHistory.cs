namespace WebSocket.Entity;

public class ProfileHistory
{
    public int WatchId { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } = null!;
}