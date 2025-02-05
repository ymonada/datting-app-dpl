namespace WebSocket.Entity;

public class Like
{
    public int Id { get; set; }
    public User UserTo { get; set; }
    public User UserFrom { get; set; } 
    public int UserToId { get; set; }
    public int UserFromId { get; set; }
}