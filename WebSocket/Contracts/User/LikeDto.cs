namespace WebSocket.Contracts.User;

public record LikeDto(Guid Id, Guid ReceiverId, Guid SenderId, string Message);