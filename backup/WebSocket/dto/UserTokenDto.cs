namespace WebSocket.dto;

public record UserTokenDto(int Id, string Email, IEnumerable<RoleDto> Roles);