using WebSocket.Domain.dto;
using WebSocket.Entity;

namespace WebSocket.Domain.Entity;

public class Match
{
    public  int Id {get; set;}
    public int FirstUserId {get; set;}
    public int SecondUserId {get; set;}
    public User FirstUser {get; set;}
    public User SecondUser {get; set;}
    
    public MatchDto ToDto() => new MatchDto(FirstUserId, SecondUserId);
}