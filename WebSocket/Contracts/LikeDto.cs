namespace WebSocket.Contracts;

public record LikeDto(Guid Id, Guid ReceiverId, Guid SenderId, string Message);