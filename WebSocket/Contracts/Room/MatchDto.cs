using WebSocket.Domain.Entity;

namespace WebSocket.Domain.dto;

public record MatchDto(Guid FirstUserId, Guid SecondUserId);