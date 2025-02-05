namespace WebSocket.Entity;

public class Photo
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } 
    public string Url { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
}