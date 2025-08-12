namespace WebSocket.Features.Photos;

public record PhotoResultDto(byte[] fileBytes, string type);